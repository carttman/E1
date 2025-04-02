using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerBuildButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private int towerIndex;
    [SerializeField] private TowerData towerData;

    [SerializeField] private Image towerImage;
    [SerializeField] private Image elementImage;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private TextMeshProUGUI towerNameText;
    
    [SerializeField] private Color canAffordColor;
    [SerializeField] private Color cannotAffordColor;
    [SerializeField] private Color canAffordTextColor;
    [SerializeField] private Color cannotAffordTextColor;
    [SerializeField] private float pointerTweenOffset = 7f;
    
    [Header("Sound")]
    [SerializeField] private AudioClip clickSound;
    
    private Button _button;
    private TooltipTrigger _tooltipTrigger;
    private float _startYPosition;
    private RectTransform _rectTransform;

    private Tween _tween;
    private Tween _goldChangeTween;
    
    private void Start()
    {
        _button = GetComponent<Button>();
        _tooltipTrigger = GetComponent<TooltipTrigger>();
        _rectTransform = GetComponent<RectTransform>();
        
        towerData = Game.Instance.TowerData[towerIndex];
        towerImage.sprite = towerData.sprite;

        elementImage.sprite = Game.Instance.GlobalData.GetElementIcon(towerData.elementType);
        
        priceText.text = towerData.goldCost.ToString();
        towerNameText.text = towerData.towerName;
        
        _tooltipTrigger.header =  towerData.towerName;
        _tooltipTrigger.content = towerData.description;
        
        _startYPosition = _rectTransform.localPosition.y;

        _button.onClick.AddListener(OnClick);
        
        Game.Instance.GoldChanged += OnGoldChanged;
        StartCoroutine(WaitForGameGoldSetup());
    }

    private void OnClick()
    {
        AudioManager.Instance.PlaySound(clickSound);
        Game.Instance.ToggleTowerBuildSelection(towerIndex);
    }

    private IEnumerator WaitForGameGoldSetup()
    {
        yield return new WaitUntil(() => Game.Instance.didStart);
        OnGoldChanged(Game.Instance.Gold);
    }

    private void OnGoldChanged(int gold)
    {
        if (!towerData) return;
        
        var canAfford = gold >= towerData.goldCost;

        if (canAfford)
        {
            _goldChangeTween?.Kill();
            _goldChangeTween = _rectTransform.DOLocalMoveY(_startYPosition, 0.2f).SetEase(Ease.OutElastic).SetLink(gameObject);
        }
        else
        {
            _tween?.Kill();
            _goldChangeTween?.Kill();
            _goldChangeTween = _rectTransform.DOLocalMoveY(_startYPosition - pointerTweenOffset, 0.2f).SetEase(Ease.OutElastic).SetLink(gameObject);
        }

        _button.interactable = canAfford;
        _button.image.color = canAfford ? canAffordColor : cannotAffordColor;
        priceText.color = canAfford ? canAffordTextColor : cannotAffordTextColor;
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!_button.interactable) return;
        _goldChangeTween?.Kill();
        _tween?.Kill();
        _tween = _rectTransform.DOLocalMoveY(_startYPosition + pointerTweenOffset, 0.2f).SetEase(Ease.OutElastic).SetLink(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!_button.interactable && _tween is { active: false }) return;
        _goldChangeTween?.Kill();
        _tween?.Kill();
        _tween = _rectTransform.DOLocalMoveY(_startYPosition, 0.2f).SetEase(Ease.OutElastic).SetLink(gameObject);
    }
}
