using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Game : MonoBehaviour
{
    public event Action<int> goldChanged;
    public event Action<int> livesChanged;

    [SerializeField] private WaveSpawner waveSpawner;
    [SerializeField] private List<GameObject> towerPrefabs;
    
    [SerializeField] private GameObject buildingTower;

    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject gameoverUI;
    
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

    private int _lives = 10;

    public int Lives
    {
        get => _lives;
        set
        {
            if (_lives == value) return;
            _lives = value;
            livesChanged?.Invoke(_lives);
            if (_lives <= 0)
            {
                GameOver();
            }
        }
    }

    private void GameOver()
    {
        mainUI.SetActive(false);
        gameoverUI.SetActive(true);
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
        // 시작시 UI 리프레시 위해 이벤트 호출
        goldChanged?.Invoke(_gold);
        livesChanged?.Invoke(_lives);
    }

    private void WaveSpawnerOnEnemySpawned(GameObject obj)
    {
        var enemy = obj.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.OnEnemyDied += OnEnemyDied;
            enemy.OnEnemyEndPath += OnEnemyEndPath;
        }
    }

    private void OnEnemyEndPath(Enemy enemy, int livesdamage)
    {
        if (enemy != null)
        {
            enemy.OnEnemyEndPath -= OnEnemyEndPath;
        }

        Lives -= livesdamage;
    }

    private void OnEnemyDied(Enemy enemy, float goldDropAmount)
    {
        if (enemy != null)
        {
            enemy.OnEnemyDied -= OnEnemyDied;
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

    public void GameRetry()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
