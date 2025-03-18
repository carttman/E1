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

    // 웨이브 스폰 레퍼런스
    [SerializeField] private WaveSpawner waveSpawner;
    
    // 타워가 사용하는 PREFABS
    [SerializeField] private List<GameObject> towerPrefabs;
    
    // UI에서 선택한 타워
    [SerializeField] private GameObject buildingTower;

    // 게임오버 처리를 위한 UI 레퍼런스
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

    // 게임 오버시 호출됨
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

    // 적이 끝 도달시 목숨에 데미지 처리
    private void OnEnemyEndPath(Enemy enemy, int livesdamage)
    {
        if (enemy != null)
        {
            enemy.OnEnemyEndPath -= OnEnemyEndPath;
        }

        Lives -= livesdamage;
    }

    // 적이 죽을시 골드 추가
    private void OnEnemyDied(Enemy enemy, float goldDropAmount)
    {
        if (enemy != null)
        {
            enemy.OnEnemyDied -= OnEnemyDied;
        }
        AddGold((int)goldDropAmount);
    }

    // 타워 UI 클릭시 건설 토글
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

    // 게임오버 UI 재시작 처리 (현재 신 다시 불러옴)
    public void GameRetry()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
}
