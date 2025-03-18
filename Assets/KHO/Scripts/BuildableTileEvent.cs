using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

// 타일의 마우스 이벤트 처리하는 컴포넌트
public class BuildableTileEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Vector3 startPosition;
    private float yOffset = 0.3f;

    private Color _startColor;
    [SerializeField] private Color highlightColor = Color.blue;

    private Renderer _renderer;
    
    public void Awake()
    {
        _renderer = GetComponent<Renderer>();
        startPosition = transform.position;
        _startColor = _renderer.material.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameEventHub.Instance.TilePointerEnter(transform);
        transform.DOComplete();
        transform.DOLocalMoveY(startPosition.y + yOffset, 0.15f).SetEase(Ease.InCubic);
        _renderer.material.DOColor(highlightColor, 0.75f).SetEase(Ease.InCubic).SetLoops(-1, LoopType.Yoyo);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameEventHub.Instance.TilePointerExit(transform);
        transform.DOComplete();
        transform.DOLocalMoveY(startPosition.y, 0.1f).SetEase(Ease.InCubic);
        _renderer.material.DOKill();
        _renderer.material.color = _startColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameEventHub.Instance.TilePointerClick(transform);
    }
}
