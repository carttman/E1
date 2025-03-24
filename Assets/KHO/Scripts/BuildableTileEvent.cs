using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

// 타일의 마우스 이벤트 처리하는 컴포넌트
public class BuildableTileEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Vector3 startPosition;
    private float yOffset = 0.5f;

    private Color _startColor;
    [SerializeField] private Color highlightColor = Color.blue;
    [SerializeField] private Renderer _renderer;
    
    public void Awake()
    {
        _renderer = GetComponentInChildren<Renderer>();
        startPosition = transform.position;
        _startColor = _renderer.material.color;
    }

    private void Start()
    {
        GameEventHub.Instance.OnStartBuildingTower += StartHighlight;
        GameEventHub.Instance.OnStopBuildingTower += StopHighlight;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameEventHub.Instance.TilePointerEnter(transform);
        //if (Time.timeScale == 0f) return;
        _renderer.transform.DOComplete();
        _renderer.transform.DOLocalMoveY(yOffset, 0.15f).SetEase(Ease.InCubic).SetUpdate(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameEventHub.Instance.TilePointerExit(transform);
        //if (Time.timeScale == 0f) return;
        _renderer.transform.DOComplete();
        _renderer.transform.DOLocalMoveY(0f, 0.1f).SetEase(Ease.InCubic).SetUpdate(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameEventHub.Instance.TilePointerClick(transform);
    }

    public void StartHighlight()
    {
        if (!_renderer) return;
        _renderer.material.DOColor(highlightColor, 0.75f).SetEase(Ease.InCubic).SetLoops(-1, LoopType.Yoyo);
    }

    public void StopHighlight()
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
