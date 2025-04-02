using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelectionUI : MonoBehaviour
{
    [Header("Tower Info")] [SerializeField]
    private TextMeshProUGUI towerNameText;

    [SerializeField] private TextMeshProUGUI towerDescriptionText;
    [SerializeField] private StarDisplayUI starDisplayUI;
    [SerializeField] private Image towerSprite;
    [SerializeField] private Image towerElementSprite;

    [Header("Tower Stats")] [SerializeField]
    private TextMeshProUGUI dpsText;

    [SerializeField] private TextMeshProUGUI damageText;
    [SerializeField] private TextMeshProUGUI attackSpeedText;
    [SerializeField] private TextMeshProUGUI rangeText;

    [Header("Tower Records")] [SerializeField]
    private TextMeshProUGUI killsText;

    [SerializeField] private TextMeshProUGUI dealtDamageText;

    [Header("Upgrade Buttons")] [SerializeField]
    private GameObject towerUpgradeButton1;

    [SerializeField] private GameObject towerUpgradeButton2;

    [Header("Targeting Type")] [SerializeField]
    private TMP_Dropdown towerTargetingType;

    [Header("Actions")] [SerializeField] private TowerDeleteButton towerDeleteButton;

    private TowerSelectionData _data;

    public void HandleUIChange(TowerSelectionData data)
    {
        _data = data;

        towerNameText.text = data.StaticTowerData.towerName;
        towerDescriptionText.text = data.StaticTowerData.description;

        towerSprite.sprite = data.StaticTowerData.sprite;
        towerElementSprite.sprite = Game.Instance.GlobalData.GetElementIcon(data.StaticTowerData.elementType);

        // STATS
        var damage = _data.StaticTowerData.TowerStats[(int)_data.tower.Rarity].damage;
        var speed = _data.StaticTowerData.TowerStats[(int)_data.tower.Rarity].attackSpeed;
        var range = _data.StaticTowerData.TowerStats[(int)_data.tower.Rarity].range;
        var dps = Mathf.Approximately(speed, 0) ? damage : damage * speed;

        dpsText.text = $"초당 데미지: {dps:0.#}";
        damageText.text = $"데미지: {damage:0.#}";
        attackSpeedText.text = $"공격속도: {speed:0.#}";
        rangeText.text = $"범위: {range:0.#}미터";

        // RECORD
        dealtDamageText.text = $"가한 데미지:\n{data.DealtDamage:0}";
        killsText.text = $"킬 수: {data.Kills}";

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

        if (_data?.tower) starDisplayUI.ChangeStar((int)_data.tower.Rarity + 1);

        towerDeleteButton.button.onClick.RemoveAllListeners();
        towerDeleteButton.button.onClick.AddListener(OnTowerDelete);
    }

    private void OnTowerDelete()
    {
        if (_data.tower == null) return;
        AudioManager.instance.PlaySound(SoundEffect.TowerDeleted);
        Game.Instance.DeleteTower(_data.tower);
    }

    public void ChangeTowerTargetingType(int newTargetingType)
    {
        if (_data == null || _data.tower == null) return;

        _data.tower.TargetingType = (Tower.EnemyCompareType)newTargetingType;
    }
}