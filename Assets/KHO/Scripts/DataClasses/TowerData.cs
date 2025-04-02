using System;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Scriptable Objects/TowerData")]
public class TowerData : ScriptableObject
{
    [SerializeField] public string towerName;
    [SerializeField] public string description;
    [SerializeField] public int goldCost;
    [SerializeField] public Sprite sprite;
    [SerializeField] public GameObject towerPrefab;
    [SerializeField] public TowerStat[] TowerStats = new TowerStat[3];
    [SerializeField] public Global.Element elementType = Global.Element.Fire;
    [SerializeField] public TowerData[] upgradesTo = new TowerData[2];

    [Serializable]
    public class TowerStat
    {
        public float damage;
        public float range;
        public float attackSpeed;
    }
}