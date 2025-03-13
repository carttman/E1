using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Game : MonoBehaviour
{
    [SerializeField] private List<GameObject> towerPrefabs;
    
    [SerializeField]
    private List<GameObject> enemies = new();

    [FormerlySerializedAs("selectedTower")] [SerializeField]
    private GameObject buildingTower;
    
    private void Awake()
    {
        GameObject.FindGameObjectsWithTag("Enemy", enemies);
    }

    public void toggleTowerBuildSelection(int button_idx)
    {
        Debug.Log($"{button_idx} was selected!");
        if (!buildingTower)
        {
            var newTower = Instantiate(towerPrefabs[button_idx]);
        }

        if (buildingTower)
        {
            Destroy(buildingTower);
            buildingTower = null;
        }
    }
}
