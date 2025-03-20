using System;
using System.Linq;
using UnityEngine;

// 타워 건설시 마우스 따라가는 표시하는 컴포넌트
public class BuildingTowerGhost : MonoBehaviour
{
    private Color _startColor;
    private Color _selectionColor = new Color(0, 1, 0, 0.25f);
    
    public event Action<TowerData> OnTowerBuilt;
    
    private Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);
    private Transform _pointerTile = null;

    private Renderer[] _renderers;

    private void Awake()
    {
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

        foreach (var rend in _renderers)
        {
            rend.material.color = _selectionColor;
        }
        
        GameEventHub.Instance.StartBuildingTower();
    }

    private void OnTilePointerClick(Transform obj)
    {
        Tower tower = GetComponent<Tower>();
        OnTowerBuilt?.Invoke(tower.towerData);
        OnTowerBuilt = null;
        enabled = false;

        foreach (var rend in _renderers)
        {
            rend.material.color = _startColor;
        }
        
        GameEventHub.Instance.StopBuildingTower();
        tower.enabled = true;
    }

    private void OnTilePointerEnter(Transform obj) => _pointerTile = obj;
    
    private void OnTilePointerExit(Transform obj)
    {
        if (_pointerTile == obj)
        {
            _pointerTile = null;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if (_pointerTile)
        {
            transform.position = _pointerTile.position;
            return;
        }
        
        if (Physics.Raycast(TouchRay, out RaycastHit hit, 5000f, LayerMask.NameToLayer("Default")))
        {
            Debug.Log("ray hit");
            transform.position = hit.point;
        }
    }
}
