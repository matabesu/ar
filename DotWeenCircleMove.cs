using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DotWeenCircleMove : MonoBehaviour
{
    [SerializeField] private float radius = 5f;          // 円の半径
    [SerializeField] private int pointsCount = 36;       // 円を構成するポイントの数
    [SerializeField] private float duration = 3f;        // 1周にかかる時間
    private Vector3[] circlePath;                        // 円軌道の座標
    private Tween tween;

    void Start()
    {
        // 円軌道を生成
        circlePath = GenerateCirclePath(radius, pointsCount);

        // オブジェクトを円周上の最初のポイントに配置
        transform.localPosition = circlePath[0];

        // DOTweenで円軌道を移動
        tween = transform.DOLocalPath(
            circlePath,
            duration,
            PathType.Linear // 線形パスを使用
        ).SetOptions(true)        // 軌道を閉じる
         .SetEase(Ease.Linear)     // 一定速度で移動
         .SetLoops(-1, LoopType.Restart) // 無限ループ
         .SetLookAt(0.01f);        // 軌道の進行方向を向く
    }

    /// <summary>
    /// 円の軌道を生成（閉じない円）
    /// </summary>
    /// <param name="radius">円の半径</param>
    /// <param name="pointsCount">円を構成するポイントの数</param>
    /// <returns>円軌道のポイント配列</returns>
    private Vector3[] GenerateCirclePath(float radius, int pointsCount)
    {
        List<Vector3> points = new List<Vector3>();

        for (int i = 0; i < pointsCount; i++)
        {
            float angle = Mathf.Deg2Rad * (360f / pointsCount) * i; // 角度をラジアンに変換
            float x = Mathf.Cos(angle) * radius;
            float z = Mathf.Sin(angle) * radius;
            points.Add(new Vector3(x, 0f, z)); // 水平面（y=0）上のポイント
        }

        return points.ToArray();
    }
}
