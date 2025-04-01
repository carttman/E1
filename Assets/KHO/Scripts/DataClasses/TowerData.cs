using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Scriptable Objects/TowerData")]
public class TowerData : ScriptableObject
{
    [SerializeField] public string towerName;
    [SerializeField] public string description;
    [SerializeField] public int goldCost;
    [SerializeField] public Sprite sprite;
    [SerializeField] public GameObject towerPrefab;
    [SerializeField] public float damage;
    [SerializeField] public float range;
    [SerializeField] public float attackSpeed;
    [SerializeField] public Global.Element elementType = Global.Element.Fire;
    [SerializeField] public TowerData[] upgradesTo = new TowerData[2];
}
