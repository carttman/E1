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
    
    [SerializeField] private Image towerImage;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI nameText;
    
    [SerializeField] private Color canAffordColor;
    [SerializeField] private Color cannotAffordColor;

    private Image _image;
    private Button _button;
    private TooltipTrigger _tooltipTrigger;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _tooltipTrigger = GetComponent<TooltipTrigger>();
    }

    private void Start()
    {
        OnEnable();
        OnGoldChanged(Game.Instance.Gold);
        Game.Instance.GoldChanged += OnGoldChanged;
    }
    
    private void OnGoldChanged(int gold)
    {
        var canAfford = gold >= towerData.goldCost;
        _image.color = canAfford ? canAffordColor : cannotAffordColor;
        _button.interactable = canAfford;
    }

    private void OnEnable()
    {
        if (towerData == null) return;
        
        towerImage.sprite = towerData.sprite;
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