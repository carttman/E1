using System;
using UnityEngine;

public class GameEventHub : MonoBehaviour
{
    public event Action<Transform> OnTilePointerEnter;
    public event Action<Transform> OnTilePointerExit;
    public event Action<Transform> OnTilePointerClick;
    
    public static GameEventHub Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
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
}
