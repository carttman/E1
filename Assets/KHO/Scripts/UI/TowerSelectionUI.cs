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
    [SerializeField] private GameObject towerUpgradeButton1;
    [SerializeField] private GameObject towerUpgradeButton2;
    
    public void HandleUIChange(TowerSelectionData data)
    {
        towerNameText.text = data.StaticTowerData.towerName;
        killsText.text = $"Kills: {data.Kills}";
        towerSprite.sprite = data.StaticTowerData.sprite;
        damageText.text = $"Damage: {data.StaticTowerData.damage}";
        dealtDamageText.text = $"Dealt Damage: {data.DealtDamage}";
        
        if (data.StaticTowerData.upgradesTo[0] != null)
        {
            var button = towerUpgradeButton1.GetComponent<TowerUpgradeButton>();
            button.tower = data.tower;
            button.towerData = data.StaticTowerData.upgradesTo[0];
            
            towerUpgradeButton1.SetActive(true);
        }
        else
        {
            towerUpgradeButton1.SetActive(false);
        }
        
        if (data.StaticTowerData.upgradesTo[1] != null)
        {
            var button = towerUpgradeButton2.GetComponent<TowerUpgradeButton>();
            button.tower = data.tower;
            button.towerData = data.StaticTowerData.upgradesTo[1];
            
            towerUpgradeButton2.SetActive(true);
        }
        else
        {
            towerUpgradeButton2.SetActive(false);
        }
    }
}