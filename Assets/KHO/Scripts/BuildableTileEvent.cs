using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

// 타일의 마우스 이벤트 처리하는 컴포넌트
public class BuildableTileEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Color highlightColor = Color.blue;
    [SerializeField] private Renderer _renderer;
    private readonly float _yOffset = 0.5f;

    private Color _startColor;

    public void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        _startColor = _renderer.material.color;
    }

    private void Start()
    {
        GameEventHub.Instance.OnStartBuildingTower += StartHighlight;
        GameEventHub.Instance.OnStopBuildingTower += StopHighlight;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameEventHub.Instance.TilePointerClick(transform);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameEventHub.Instance.TilePointerEnter(transform);
        _renderer.transform.DOComplete();
        _renderer.transform.DOLocalMoveY(_yOffset, 0.15f).SetEase(Ease.InCubic).SetUpdate(true).SetLink(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameEventHub.Instance.TilePointerExit(transform);
        _renderer.transform.DOComplete();
        _renderer.transform.DOLocalMoveY(0f, 0.1f).SetEase(Ease.InCubic).SetUpdate(true).SetLink(gameObject);
    }

    private void StartHighlight()
    {
        if (!_renderer) return;
        _renderer.material.DOColor(highlightColor, 0.75f).SetEase(Ease.InCubic).SetLoops(-1, LoopType.Yoyo);
    }

    private void StopHighlight()
    {
        if (!_renderer) return;
        _renderer.material.DOKill(true);
        _renderer.material.color = _startColor;
    }

    public void Disable()
    {
        _renderer.transform.DOKill(true);
        _renderer.transform.localPosition = Vector3.zero;

        StopHighlight();

        GameEventHub.Instance.OnStartBuildingTower -= StartHighlight;
        GameEventHub.Instance.OnStopBuildingTower -= StopHighlight;

        enabled = false;
    }
}