using System;
using UnityEngine;

public class UIBillboarding : MonoBehaviour
{
    private Camera MyCamera;
    private void Awake()
    {
        MyCamera = Camera.main;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.forward = MyCamera.transform.forward;
    }
}
