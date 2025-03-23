using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class TowerSelectionData: SelectionData
{
    public TowerSelectionData(TowerData staticTowerData, int kills, float dealtDamage)
    {
        StaticTowerData = staticTowerData;
        this.kills = kills;
        this.dealtDamage = dealtDamage;
    }

    public readonly TowerData StaticTowerData;
    
    [SerializeField] private int kills;
    public int Kills
    {
        get => kills;
        set
        {
            if (!Equals(kills, value))
            {
                kills = value;
                OnSelectionDataChanged?.Invoke(this);
            }
        }
    }
    
    [SerializeField] private float dealtDamage;
    public float DealtDamage
    {
        get => dealtDamage;
        set
        {
            if (!Mathf.Approximately(dealtDamage, value))
            {
                dealtDamage = value;
                OnSelectionDataChanged?.Invoke(this);
            }
        }
    }
}
