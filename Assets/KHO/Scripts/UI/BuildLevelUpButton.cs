using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildLevelUpButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Image goldImage;
    
    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        Game.Instance.BuildLevelChanged += OnBuildLevelChanged;
        Game.Instance.GoldChanged += OnGoldChanged;
        _button.onClick.AddListener(OnClick);
        OnGoldChanged(Game.Instance.Gold);
        OnBuildLevelChanged(Game.Instance.BuildLevel);
    }

    private void OnClick()
    {
        Game.Instance.BuildLevelUp();
    }

    private void OnGoldChanged(int newGold)
    {
        if (!Game.Instance.CanLevelUpBuildLevel)
        {
            _button.interactable = false;
            return;
        }
        bool buyable = Game.Instance.GlobalData.towerLevelUpCost[Game.Instance.BuildLevel] <= newGold;
        _button.interactable = buyable;
    }

    private void OnBuildLevelChanged(int newBuildLevel)
    {
        descriptionText.text = newBuildLevel < Game.Instance.MaxBuildLevel ?  $"건설 레벨 {newBuildLevel}->{newBuildLevel+1}" : $"건설 레벨 {newBuildLevel}";

        if (newBuildLevel >= Game.Instance.MaxBuildLevel)
        {
            goldImage.gameObject.SetActive(false);
        }
        else
        {
            goldText.text = Game.Instance.GlobalData.towerLevelUpCost[newBuildLevel].ToString();
        }
    }
}
