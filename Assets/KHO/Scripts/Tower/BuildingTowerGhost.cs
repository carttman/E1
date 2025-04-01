using System;
using System.Linq;
using UnityEngine;

// 타워 건설시 마우스 따라가는 표시하는 컴포넌트
public class BuildingTowerGhost : MonoBehaviour
{
    public event Action<TowerData> OnTowerBuilt;
    
    [SerializeField] private Color _cannotBuildColor = new Color(1, 0, 0, 0.25f);
    [SerializeField] private Color _canBuildColor = new Color(0, 1, 0, 0.25f);
   
    private Color _startColor;
    
    private static Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);
    private Transform _pointerTile = null;
    private Renderer[] _renderers;

    void Update()
    {
        // 마우스 닿은 타일이 있을 경우 타일에 스냅
        if (_pointerTile != null)
        {
            transform.position = _pointerTile.position;
            return;
        }
        
        // 아닐시 바닥에서 마우스 따라감
        if (Physics.Raycast(TouchRay, out RaycastHit hit, 5000f, LayerMask.GetMask("Default")))
        {
            //Debug.Log("ray hit");
            transform.position = hit.point;
        }
    }
    
    private void Awake()
    {
        // 태그 비교로 맞는 렌더러만 색깔 바꿈
        _renderers = GetComponentsInChildren<Renderer>().Where(r => r.CompareTag("TowerMesh")).ToArray();
        if (_renderers != null && _renderers.Length > 0)
        {
            _startColor = _renderers[0].material.color;
        }
    }

    private void Start()
    {
        GameEventHub.Instance.OnTilePointerEnter += OnTilePointerEnter;
        GameEventHub.Instance.OnTilePointerExit += OnTilePointerExit;
        GameEventHub.Instance.OnTilePointerClick += OnTilePointerClick;

        ChangeColor(_cannotBuildColor);
        
        GameEventHub.Instance.StartBuildingTower();
    }

    private void OnTilePointerClick(Transform obj)
    {
        GameEventHub.Instance.OnTilePointerEnter -= OnTilePointerEnter;
        GameEventHub.Instance.OnTilePointerExit -= OnTilePointerExit;
        GameEventHub.Instance.OnTilePointerClick -= OnTilePointerClick;
        
        obj.GetComponent<BuildableTileEvent>().Disable();
        
        Tower tower = GetComponent<Tower>();
        OnTowerBuilt?.Invoke(tower.towerData);
        OnTowerBuilt = null;
        
        tower.ChangeRarity(Game.Instance.RollTowerRarity());
        
        enabled = false;
        
        ChangeColor(_startColor);
        
        GameEventHub.Instance.StopBuildingTower();
        tower.enabled = true;
    }

    private void OnDestroy()
    {
        GameEventHub.Instance.OnTilePointerEnter -= OnTilePointerEnter;
        GameEventHub.Instance.OnTilePointerExit -= OnTilePointerExit;
        GameEventHub.Instance.OnTilePointerClick -= OnTilePointerClick;
        
        GameEventHub.Instance.StopBuildingTower();
    }

    private void OnTilePointerEnter(Transform obj)
    {
        _pointerTile = obj;
        ChangeColor(_canBuildColor);
    }
    
    private void OnTilePointerExit(Transform obj)
    {
        if (_pointerTile == obj)
        {
            _pointerTile = null;
            ChangeColor(_cannotBuildColor);
        }
    }

    private void ChangeColor(Color _color)
    {
        foreach (var rend in _renderers)
        {
            rend.material.color = _color;
        }
    }
}
