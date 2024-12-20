using UnityEngine;

/// <summary>
/// 3Dオブジェクトに円運動をアタッチする
/// </summary>
public class UseRotateAround : MonoBehaviour
{
    [SerializeField] GameObject center;
    //円運動の速度
    float speed = 20;

    // Update is called once per frame
    void Update()
    {

        //RotateAround(円運動の中心,進行方向,速度)
        transform.RotateAround(center.transform.position,
        transform.forward, speed * Time.deltaTime);

    }
}
