using System;
using UnityEngine;

public class PopUpUIBillboard : MonoBehaviour
{
    private Camera MyCamera;
    private void Awake()
    {
        MyCamera = Camera.main;
        transform.forward = MyCamera.transform.forward;
    }
    void Update()
    {
        transform.forward = MyCamera.transform.forward;
    }
}
