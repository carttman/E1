using System;
using UnityEngine;

// 이벤트 처리하는 컴포넌트
public class GameEventHub : MonoBehaviour
{
    public static GameEventHub Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void TilePointerEnter(Transform tile)
    {
        OnTilePointerEnter?.Invoke(tile);
    }

    public void TilePointerExit(Transform tile)
    {
        OnTilePointerExit?.Invoke(tile);
    }

    public void TilePointerClick(Transform tile)
    {
        OnTilePointerClick?.Invoke(tile);
    }

    public void StartBuildingTower()
    {
        OnStartBuildingTower?.Invoke();
    }

    public void StopBuildingTower()
    {
        OnStopBuildingTower?.Invoke();
    }

    public event Action<Transform> OnTilePointerEnter;
    public event Action<Transform> OnTilePointerExit;
    public event Action<Transform> OnTilePointerClick;

    public event Action OnStartBuildingTower;
    public event Action OnStopBuildingTower;
}