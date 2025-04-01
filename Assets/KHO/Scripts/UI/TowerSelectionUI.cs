using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelectionUI : MonoBehaviour
{
    private TowerSelectionData _data;
    
    [SerializeField] private TextMeshProUGUI towerNameText;
    [SerializeField] private TextMeshProUGUI towerDescriptionText;
    [SerializeField] private TextMeshProUGUI killsText;
    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI dealtDamageText;
    [SerializeField] private Image towerSprite;
    [SerializeField] private Image towerElementSprite;
    [SerializeField] private GameObject towerUpgradeButton1;
    [SerializeField] private GameObject towerUpgradeButton2;
    [SerializeField] private TMP_Dropdown towerTargetingType;
    
    public void HandleUIChange(TowerSelectionData data)
    {
        _data = data;
        
        towerNameText.text = data.StaticTowerData.towerName;
        towerDescriptionText.text = data.StaticTowerData.description;
        killsText.text = $"킬 수: {data.Kills}";
        
        towerSprite.sprite = data.StaticTowerData.sprite;
        towerElementSprite.sprite = Game.Instance.GlobalData.GetElementIcon(data.StaticTowerData.elementType);
        
        damageText.text = $"데미지: {data.StaticTowerData.damage}";
        dealtDamageText.text = $"가한 데미지:\n{data.DealtDamage:0}";
        
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

        if (_data?.tower?.TargetingType != null) towerTargetingType.value = (int)_data.tower.TargetingType;
    }

    public void ChangeTowerTargetingType(Int32 newTargetingType)
    {
        if (_data == null || _data.tower == null)
        {
            return;
        }
        
        _data.tower.TargetingType = (Tower.EnemyCompareType) newTargetingType;
    }
}