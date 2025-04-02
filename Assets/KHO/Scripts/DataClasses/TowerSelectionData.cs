using System;
using UnityEngine;

[Serializable]
public class TowerSelectionData : SelectionData
{
    public Tower tower;

    [SerializeField] private int kills;

    [SerializeField] private float dealtDamage;

    public readonly TowerData StaticTowerData;

    public TowerSelectionData(TowerData staticTowerData, int kills, float dealtDamage)
    {
        StaticTowerData = staticTowerData;
        this.kills = kills;
        this.dealtDamage = dealtDamage;
    }

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