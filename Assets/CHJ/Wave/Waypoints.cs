using System;
using UnityEngine;
using UnityEngine.UIElements;

public class Waypoints : MonoBehaviour
{
    public static Transform[] points;

    void Awake()
    {
        points = new Transform[transform.childCount]; // 웨이포인트들의 트랜스폼  
        for (int i = 0; i < points.Length; i++) // 각 인덱스에 웨이포인트 트랜스폼 대입
        {
           points[i] = transform.GetChild(i);
        }
    }
}
