using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TowerUpgradeButton : MonoBehaviour
{
    [SerializeField] public Tower tower;
    [SerializeField] public TowerData towerData;

    [FormerlySerializedAs("_image")] [SerializeField] private Image image;
    [FormerlySerializedAs("_priceText")] [SerializeField] private TextMeshProUGUI priceText;
    [FormerlySerializedAs("_nameText")] [SerializeField] private TextMeshProUGUI nameText;
    
    private Button _button;
    private TooltipTrigger _tooltipTrigger;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _tooltipTrigger = GetComponent<TooltipTrigger>();
    }

    private void Start()
    {
        OnEnable();
    }
    
    private void OnEnable()
    {
        if (towerData == null) return;
        
        image.sprite = towerData.sprite;
        priceText.text = towerData.goldCost.ToString();
        nameText.text = towerData.towerName;
        
        _tooltipTrigger.header = towerData.towerName;
        _tooltipTrigger.content = towerData.description;
        
        _button.onClick.AddListener(() => Game.Instance.UpgradeTower(tower, towerData));
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }
}