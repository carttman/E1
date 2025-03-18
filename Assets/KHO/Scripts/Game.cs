using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Game : MonoBehaviour
{
    public event Action<int> goldChanged;

    [SerializeField] private WaveSpawner waveSpawner;
    [SerializeField] private List<GameObject> towerPrefabs;
    
    [SerializeField]
    private GameObject buildingTower;

    [SerializeField] private TowerData[] towerDatas;
    
    private int _gold = 10;
    public int Gold
    {
        get => _gold;
        set
        {
            if (_gold == value) return;
            _gold = value;
            goldChanged?.Invoke(_gold);
        }
    }
    
    private bool CanBuyTower(int idx) => towerDatas[idx].goldCost <= Gold;

    public void AddGold(int amount)
    {
        Gold += amount;
    }

    public void SpendGold(int amount)
    {
        Gold -= amount;
    }
    
    private void Awake()
    {
        waveSpawner.OnEnemySpawned += WaveSpawnerOnEnemySpawned;
    }

    private void Start()
    {
        goldChanged?.Invoke(_gold);
    }

    private void WaveSpawnerOnEnemySpawned(GameObject obj)
    {
        var enemy = obj.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.EnemyDied += OnEnemyDied;
        }
    }

    private void OnEnemyDied(Enemy enemy, float goldDropAmount)
    {
        if (enemy != null)
        {
            enemy.EnemyDied -= OnEnemyDied;
        }
        AddGold((int)goldDropAmount);
    }

    public void toggleTowerBuildSelection(int button_idx)
    {
        Debug.Log($"{button_idx} was selected!");
        if (!buildingTower)
        {
            if (CanBuyTower(button_idx))
            {
                var newTower = Instantiate(towerPrefabs[button_idx]);
                newTower.GetComponent<SelectionTower>().OnTowerBuilt += data => SpendGold(data.goldCost);
            }
            else
            {
                Debug.Log($"Cannot buy tower {button_idx}, not enough gold");
            }
        }

        if (buildingTower)
        {
            Destroy(buildingTower);
            buildingTower = null;
        }
    }
}
