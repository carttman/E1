using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerBuildButtonUI : MonoBehaviour
{
    [SerializeField] private int _towerIndex;
    [SerializeField] private TowerData _towerData;

    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _priceText;
    private Button _button;
    
    private void Start()
    {
        _button = GetComponent<Button>();
        
        _towerData = Game.Instance.towerDatas[_towerIndex];
        _image.sprite = _towerData.sprite;
        _priceText.text = _towerData.goldCost.ToString();

        _button.onClick.AddListener(() => Game.Instance.ToggleTowerBuildSelection(_towerIndex));
    }
}
