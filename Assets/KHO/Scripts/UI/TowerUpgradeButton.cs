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
    
    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _priceText;
    
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
        
        _image.sprite = towerData.sprite;
        _priceText.text = towerData.goldCost.ToString();
        
        _tooltipTrigger.header = towerData.name;
        _tooltipTrigger.content = towerData.description;
        
        _button.onClick.AddListener(() => Game.Instance.UpgradeTower(tower, towerData));
    }
}