using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TowerDeleteButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float pointerTweenOffset = 7f;

    public Button button;
    private Tween _tween;
    private float _startYPosition;
    private RectTransform _rectTransform;

    private void Awake()
    {
        button = GetComponent<Button>();
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _startYPosition = _rectTransform.localPosition.y;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!button.interactable) return;
        _tween?.Kill();
        _tween = _rectTransform.DOLocalMoveY(_startYPosition + pointerTweenOffset, 0.2f).SetEase(Ease.OutElastic)
            .SetLink(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!button.interactable && _tween is { active: false }) return;
        _tween?.Kill();
        _tween = _rectTransform.DOLocalMoveY(_startYPosition, 0.2f).SetEase(Ease.OutElastic).SetLink(gameObject);
    }
   
}
