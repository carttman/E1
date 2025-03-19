using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelectionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _towerNameText;
    [SerializeField] private TextMeshProUGUI _killsText;
    [SerializeField] private TextMeshProUGUI _damageText;
    [SerializeField] private Image _towerSprite;

    private void Awake()
    {
        Debug.Assert(_killsText);
        Debug.Assert(_towerSprite);
        Debug.Assert(_towerNameText);
        Debug.Assert(_damageText);
    }

    public void HandleUIChange(TowerSelectionData data)
    {
        _towerNameText.text = data.staticTowerData.towerName;
        _killsText.text = $"Kills: {data.Kills}";
        _towerSprite.sprite = data.staticTowerData.sprite;
        _damageText.text = $"Damage: {data.staticTowerData.damage}";
    }
}
