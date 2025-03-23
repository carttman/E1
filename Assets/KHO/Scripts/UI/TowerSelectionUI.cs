using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelectionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI dealtDamageText;
    [SerializeField] private Image towerSprite;
    
    public void HandleUIChange(TowerSelectionData data)
    {
        towerNameText.text = data.StaticTowerData.towerName;
        killsText.text = $"Kills: {data.Kills}";
        towerSprite.sprite = data.StaticTowerData.sprite;
        damageText.text = $"Damage: {data.StaticTowerData.damage}";
        dealtDamageText.text = $"Dealt Damage: {data.DealtDamage}";
    }
}