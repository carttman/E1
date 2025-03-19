using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    private static PopUpManager _instance;
    public static PopUpManager Instance
    {
        get { return _instance; }
    }
    
    private TMP_Text PopUpText;
    private Canvas PopUpCanvas;
    
    private void Awake()
    {
        _instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopUpDamageUI(float damage, Transform objectTransform)
    {
        //Debug.Log($"Damage: {damage}, Object: {objectTransform.position}");
        
    }
    
}
