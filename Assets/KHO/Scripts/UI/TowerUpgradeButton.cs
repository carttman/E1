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
    
    private void Start()
    {
        _button = GetComponent<Button>();

        _image.sprite = towerData.sprite;
        _priceText.text = towerData.goldCost.ToString();

        _button.onClick.AddListener(() => Game.Instance.UpgradeTower(tower, towerData));
    }
}