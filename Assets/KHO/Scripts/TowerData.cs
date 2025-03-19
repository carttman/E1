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
}
