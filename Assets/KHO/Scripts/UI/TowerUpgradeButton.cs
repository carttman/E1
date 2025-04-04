using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerUpgradeButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] public Tower tower;
    [SerializeField] public TowerData towerData;

    [SerializeField] private Image towerImage;
    [SerializeField] private Image elementImage;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI nameText;

    [SerializeField] private Color canAffordColor;
    [SerializeField] private Color cannotAffordColor;
    [SerializeField] private Color canAffordTextColor;
    [SerializeField] private Color cannotAffordTextColor;
    [SerializeField] private float pointerTweenOffset = 7f;
    private Button _button;

    private Image _image;

    private RectTransform _rectTransform;
    private float _startYPosition;
    private TooltipTrigger _tooltipTrigger;
    private Tween _tween;

    private void Awake()
    {
        _image = GetComponent<Image>();
        _button = GetComponent<Button>();
        _tooltipTrigger = GetComponent<TooltipTrigger>();
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        OnEnable();
        Game.Instance.GoldChanged += OnGoldChanged;
        OnGoldChanged(Game.Instance.Gold);
    }

    private void OnEnable()
    {
        if (towerData == null) return;

        towerImage.sprite = towerData.sprite;
        elementImage.sprite = Game.Instance.GlobalData.GetElementIcon(towerData.elementType);
        priceText.text = towerData.goldCost.ToString();
        nameText.text = towerData.towerName;

        _tooltipTrigger.header = towerData.towerName;
        _tooltipTrigger.content = towerData.description;

        _button.onClick.RemoveAllListeners();
        _button.onClick.AddListener(OnClick);
        OnGoldChanged(Game.Instance.Gold);
    }

    private void OnClick()
    {
        AudioManager.instance.PlaySound(SoundEffect.ButtonClick);
        Game.Instance.UpgradeTower(tower, towerData);
    }

    private void OnDisable()
    {
        _button.onClick.RemoveAllListeners();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_button.interactable) return;
        _tween?.Kill();
        _tween = _rectTransform.DOLocalMoveY(_startYPosition + pointerTweenOffset, 0.2f).SetEase(Ease.OutElastic)
            .SetLink(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_button.interactable && _tween is { active: false }) return;
        _tween?.Kill();
        _tween = _rectTransform.DOLocalMoveY(_startYPosition, 0.2f).SetEase(Ease.OutElastic).SetLink(gameObject);
    }

    private void OnGoldChanged(int gold)
    {
        var canAfford = gold >= towerData.goldCost;
        _image.color = canAfford ? canAffordColor : cannotAffordColor;
        priceText.color = canAfford ? canAffordTextColor : cannotAffordTextColor;
        _button.interactable = canAfford;
    }
}