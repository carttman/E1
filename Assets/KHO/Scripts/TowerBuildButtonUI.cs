using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerBuildButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int _towerIndex;
    [SerializeField] private TowerData _towerData;

    [SerializeField] private Image _image;
    [SerializeField] private TextMeshProUGUI _priceText;
    [SerializeField] private TextMeshProUGUI _towerNameText;
    
    private Button _button;
    private TooltipTrigger _tooltipTrigger;
    private float _startYPosition;
    private RectTransform _rectTransform;

    private Tween _tween;
    
    private void Start()
    {
        _button = GetComponent<Button>();
        _tooltipTrigger = GetComponent<TooltipTrigger>();
        _rectTransform = GetComponent<RectTransform>();
        
        _towerData = Game.Instance.towerDatas[_towerIndex];
        _image.sprite = _towerData.sprite;
        _priceText.text = _towerData.goldCost.ToString();
        _towerNameText.text = _towerData.towerName;
        
        _tooltipTrigger.header =  _towerData.towerName;
        _tooltipTrigger.content = _towerData.description;
        
        _startYPosition = _rectTransform.localPosition.y;

        _button.onClick.AddListener(() => Game.Instance.ToggleTowerBuildSelection(_towerIndex));
    }


    public void OnPointerEnter(PointerEventData eventData)
    {
        _tween?.Kill();
        _tween = _rectTransform.DOLocalMoveY(_startYPosition + 7f, 0.2f).SetEase(Ease.InOutCubic).SetLink(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _tween?.Kill();
        _tween = _rectTransform.DOLocalMoveY(_startYPosition, 0.2f).SetEase(Ease.InOutCubic).SetLink(gameObject);
    }
}
