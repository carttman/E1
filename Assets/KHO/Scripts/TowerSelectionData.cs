using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class TowerSelectionData: SelectionData
{
    public TowerSelectionData(TowerData towerData, int kills)
    {
        staticTowerData = towerData;
        this._kills = kills;
    }
    
    public readonly TowerData staticTowerData;
    
    [SerializeField]
    private int _kills;
    public int Kills
    {
        get => _kills;
        set
        {
            if (!Equals(_kills, value))
            {
                _kills = value;
                OnSelectionDataChanged?.Invoke(this);
            }
        }
    }
}
