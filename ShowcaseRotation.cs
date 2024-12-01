using System.Collections;
using UnityEngine;

public class ShowcaseRotation : MonoBehaviour
{
    public float rotationSpeed = 10f;           // 常時回転する速度の設定
    public Light directionalLight;              // Directional Light の参照
    public Material[] targetMaterials;          // 制御する複数のマテリアル
    public float fadeDuration = 5f;             // Directional Light の点灯/消灯にかかる時間
    public float waitDuration = 2f;             // Emission が消灯状態を維持する時間
    public float fadeInDuration = 3f;           // Emission が点灯するのにかかる時間
    public float fadeOutDuration = 5f;          // Emission が消灯するのにかかる時間
    public float lightOnIntensity = 1f;         // Directional Light の点灯時の明るさ
    public float lightOffIntensity = 0f;        // Directional Light の消灯時の明るさ
    public Color emissionOnColor = Color.white; // 点灯時の Emission の色
    public Color emissionOffColor = Color.black; // 消灯時の Emission の色
    public ParticleSystem particleSystem;       // パーティクルシステムの参照

    private bool isLightOn = true;              // 起動時の点灯状態 (Emission は消灯)
    private bool fadeInProgress = false;        // フェード処理が進行中かどうかを判定するフラグ
    private bool isRotating = false;            // 回転中かどうかのフラグ
    private bool isParticlePlaying = false;     // パーティクルシステムの状態

    void Start()
    {
        // 起動時の初期状態を設定
        foreach (var material in targetMaterials)
        {
            material.EnableKeyword("_EMISSION");
            material.SetColor("_EmissionColor", emissionOffColor); // 起動時は Emission を消灯
        }
        directionalLight.intensity = lightOnIntensity; // 起動時は Directional Light を点灯

        // パーティクルシステムを停止しておく
        if (particleSystem != null)
        {
            particleSystem.Stop();
        }
    }

    void Update()
    {
        // 初回のタッチで回転を開始
        if (Input.GetMouseButtonDown(0) && !isRotating)
        {
            isRotating = true;
        }

        // タップで点灯・消灯の切り替えとパーティクルシステムのオン・オフ
        if (Input.GetMouseButtonDown(0) && !fadeInProgress)
        {
            isLightOn = !isLightOn;  // 点灯・消灯を反転
            StartCoroutine(FadeLightAndEmission(isLightOn));

            // 点灯に切り替わったら即座にパーティクルシステムをオフにする
            if (isLightOn && particleSystem.isPlaying)
            {
                particleSystem.Stop();
                isParticlePlaying = false;
            }
            // 消灯に切り替わったら2秒後にパーティクルシステムを開始する
            else if (!isLightOn && !particleSystem.isPlaying)
            {
                StartCoroutine(StartParticleWithDelay(2f));
                isParticlePlaying = true;
            }
        }

        // 回転中のみ回転させる
        if (isRotating)
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
    }

    private IEnumerator StartParticleWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        particleSystem.Play();
    }

    IEnumerator FadeLightAndEmission(bool turnOn)
    {
        fadeInProgress = true;

        float startLightIntensity = directionalLight.intensity;
        float targetLightIntensity = turnOn ? lightOnIntensity : lightOffIntensity; // 点灯時と消灯時の明るさを定数で制御
        Color targetEmissionColor = turnOn ? emissionOffColor : emissionOnColor; // turnOn が true の場合は Emission を消灯、false の場合は点灯

        // Emission に使用する時間を選択
        float emissionDuration = turnOn ? fadeOutDuration : fadeInDuration;

        // fadeDuration秒で Directional Light と Emission を切り替え
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float progress = t / fadeDuration;

            // 緩やかな進行のためのイージング
            float easedProgress = Mathf.SmoothStep(0, 1, progress);

            // Directional Light の明るさを変更
            directionalLight.intensity = Mathf.Lerp(startLightIntensity, targetLightIntensity, easedProgress);

            // Emission の色を変更（指定された waitDuration 後に fadeInDuration または fadeOutDuration でゆっくり点灯または消灯）
            if (progress >= waitDuration / fadeDuration)
            {
                float emissionStart = waitDuration / fadeDuration;
                float emissionProgress = (progress - emissionStart) / (emissionDuration / fadeDuration);
                float emissionEasedProgress = Mathf.SmoothStep(0, 1, Mathf.Clamp01(emissionProgress));

                // 各マテリアルの Emission を変更
                foreach (var material in targetMaterials)
                {
                    Color currentEmission = Color.Lerp(emissionOffColor, targetEmissionColor, emissionEasedProgress);
                    material.SetColor("_EmissionColor", currentEmission);
                }
            }

            yield return null;
        }

        // 最終状態を設定
        directionalLight.intensity = targetLightIntensity;
        foreach (var material in targetMaterials)
        {
            material.SetColor("_EmissionColor", targetEmissionColor);
        }

        fadeInProgress = false;
    }
}
