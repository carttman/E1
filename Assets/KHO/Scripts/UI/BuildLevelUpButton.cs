using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BuildLevelUpButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private Image goldImage;

    [SerializeField] private Color canAffordColor;
    [SerializeField] private Color cannotAffordColor;
    [SerializeField] private Color canAffordTextColor;
    [SerializeField] private Color cannotAffordTextColor;
    [SerializeField] private float pointerTweenOffset = 7f;

    private Button _button;
    private RectTransform _rectTransform;
    private float _startYPosition;
    private Tween _tween;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        Game.Instance.BuildLevelChanged += OnBuildLevelChanged;
        Game.Instance.GoldChanged += OnGoldChanged;
        _button.onClick.AddListener(OnClick);
        OnGoldChanged(Game.Instance.Gold);
        OnBuildLevelChanged(Game.Instance.BuildLevel);

        _startYPosition = _rectTransform.localPosition.y;
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

    private void OnClick()
    {
        AudioManager.instance.PlaySound(SoundEffect.ButtonClick);
        AudioManager.instance.PlaySound(SoundEffect.BuildLevelUp);
        Game.Instance.BuildLevelUp();
    }

    private void OnGoldChanged(int newGold)
    {
        if (!Game.Instance.CanLevelUpBuildLevel)
        {
            _button.interactable = false;
            _button.image.color = cannotAffordColor;
            goldText.color = cannotAffordTextColor;
            return;
        }

        var buyable = Game.Instance.GlobalData.towerLevelUpCost[Game.Instance.BuildLevel] <= newGold;
        _button.interactable = buyable;
        _button.image.color = buyable ? canAffordColor : cannotAffordColor;
        goldText.color = buyable ? canAffordTextColor : cannotAffordTextColor;
    }

    private void OnBuildLevelChanged(int newBuildLevel)
    {
        descriptionText.text = newBuildLevel < Game.Instance.MaxBuildLevel
            ? $"건설 레벨 {newBuildLevel}->{newBuildLevel + 1}"
            : $"건설 레벨 {newBuildLevel}";

        if (newBuildLevel >= Game.Instance.MaxBuildLevel)
            goldImage.gameObject.SetActive(false);
        else
            goldText.text = Game.Instance.GlobalData.towerLevelUpCost[newBuildLevel].ToString();

        OnGoldChanged(Game.Instance.Gold);
    }
}