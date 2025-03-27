using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Waypoints : MonoBehaviour
{
    public static Transform[] PointTransforms;

    void Awake()
    {
        PointTransforms = new Transform[transform.childCount]; // 웨이포인트들의 트랜스폼  
        for (int i = 0; i < PointTransforms.Length; i++) // 각 인덱스에 웨이포인트 트랜스폼 대입
        {
           PointTransforms[i] = transform.GetChild(i);
        }
    }
}
