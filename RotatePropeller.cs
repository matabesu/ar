using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePropeller : MonoBehaviour
{
    public float rotationSpeed = 200000f; // 高速回転用の定数

    void Update()
    {
        // Time.deltaTime を外すことでフレームごとに固定の回転を適用
        transform.Rotate(0, rotationSpeed, 0);
    }
}
