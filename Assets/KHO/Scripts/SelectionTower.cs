using System;
using UnityEngine;

public class SelectionTower : MonoBehaviour
{
    public event Action<TowerData> OnTowerBuilt;
    
    private Ray TouchRay => Camera.main.ScreenPointToRay(Input.mousePosition);
    private Transform _pointerTile = null;
    
    private void Start()
    {
        GameEventHub.Instance.OnTilePointerEnter += OnTilePointerEnter;
        GameEventHub.Instance.OnTilePointerExit += OnTilePointerExit;
        GameEventHub.Instance.OnTilePointerClick += OnTilePointerClick;
    }

    private void OnTilePointerClick(Transform obj)
    {
        Tower tower = GetComponent<Tower>();
        OnTowerBuilt?.Invoke(tower.towerData);
        OnTowerBuilt = null;
        enabled = false;
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
        
        if (Physics.Raycast(TouchRay, out RaycastHit hit))
        {
            transform.position = hit.point;
        }
    }
}
