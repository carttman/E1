using UnityEngine;

[CreateAssetMenu(fileName = "TowerData", menuName = "Scriptable Objects/TowerData")]
public class TowerData : ScriptableObject
{
    [SerializeField] public string towerName;
    [SerializeField] public string description;
    [SerializeField] public int goldCost;
    [SerializeField] public Texture2D sprite;
}
