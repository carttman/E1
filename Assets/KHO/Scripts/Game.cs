using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class Game : MonoBehaviour
{
    public float tooltipDelay = 0.1f;
    public event Action<int> GoldChanged;
    public event Action<int> LivesChanged;
    
    public static Game Instance { get; private set; }
    
    [SerializeField] public GameObject explosionPrefab;
    [SerializeField] private TowerData[] towerData;
    public TowerData[] TowerData => towerData;
    
    // 웨이브 스폰 레퍼런스
    [SerializeField] private WaveSpawner waveSpawner;
    
    // UI에서 선택한 타워
    [SerializeField] private GameObject buildingTower;

    // 게임오버 처리를 위한 UI 레퍼런스
    [SerializeField] private GameObject mainUI;
    [SerializeField] private GameObject gameoverUI;
    [SerializeField] private GameObject pauseButtonUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject clearUI;
    
    private float _beforePauseTimeScale = 1f;
    
    private int _gold = 1000;
    public int Gold
    {
        get => _gold;
        set
        {
            if (_gold == value) return;
            _gold = value;
            GoldChanged?.Invoke(_gold);
        }
    }

    private int _lives = 1000;

    public int Lives
    {
        get => _lives;
        set
        {
            if (_lives == value) return;
            _lives = value;
            LivesChanged?.Invoke(_lives);
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

    private bool CanBuyTower(int idx) => towerData[idx].goldCost <= Gold;
    private bool CanBuyTower(TowerData towerData) => towerData.goldCost <= Gold;

    private void Awake()
    {
        if (Instance != null)
        {
            Instance = null;
        }

        Instance = this;
        waveSpawner.OnEnemySpawned += WaveSpawnerOnEnemySpawned;
    }

    private void Start()
    {
        // 시작시 UI 리프레시 위해 이벤트 호출
        GoldChanged?.Invoke(_gold);
        LivesChanged?.Invoke(_lives);
        Time.timeScale = 1f;
    }
    
    // 게임오버 UI 재시작 처리 (현재 신 다시 불러옴)
    public void GameRetry()
    {
        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }
    
    public void ToMainMenu()
    {
        var asyncLoad = SceneManager.LoadSceneAsync("Map/MainMap");
    }

    public void SetTimeScale(float newTimeScale)
    {
        if (newTimeScale < 0f) return;
        Time.timeScale = newTimeScale;
    }

    public void PauseGame()
    {
        _beforePauseTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        pauseButtonUI.SetActive(false);
        pauseUI.SetActive(true);
    }
    
    public void UnPauseGame()
    {
        Time.timeScale = _beforePauseTimeScale;
        pauseButtonUI.SetActive(true);
        pauseUI.SetActive(false);
    }

    public void UpgradeTower(Tower tower, TowerData towerData)
    {
        if (tower == null) return;
        if (!CanBuyTower(towerData)) return;

        Gold -= towerData.goldCost;
        tower.UpgradeTo(towerData);
        
        TooltipSystem.Hide();
    }
    
    public void OnClickExit()
    {
        if (Application.isEditor)
        {
            Debug.Log("Application would quit here in a build");
        }
        Application.Quit();
    }
    
    public void ClearGame()
    {
        _beforePauseTimeScale = Time.timeScale;
        Time.timeScale = 0f;
        clearUI.SetActive(true);
    }

    private void WaveSpawnerOnEnemySpawned(GameObject obj)
    {
        var enemy = obj.GetComponent<Enemy>();
        if (enemy)
        {
            enemy.OnEnemyDied += OnEnemyDied;
            enemy.OnEnemyEndPath += OnEnemyEndPath;
        }
    }

    // 적이 끝 도달시 목숨에 데미지 처리
    private void OnEnemyEndPath(Enemy enemy, int livesdamage)
    {
        if (enemy)
        {
            enemy.OnEnemyEndPath -= OnEnemyEndPath;
        }

        Lives -= livesdamage;
    }

    // 적이 죽을시 골드 추가
    private void OnEnemyDied(Enemy enemy, float goldDropAmount)
    {
        if (enemy)
        {
            enemy.OnEnemyDied -= OnEnemyDied;
        }

        Gold += (int)goldDropAmount;
    }

    // 타워 UI 클릭시 건설 토글
    public void ToggleTowerBuildSelection(int button_idx)
    {
        //Debug.Log($"{button_idx} was selected!");
        if (!buildingTower)
        {
            if (CanBuyTower(button_idx))
            {
                var newTower = Instantiate(towerData[button_idx].towerPrefab);
                newTower.GetComponent<BuildingTowerGhost>().OnTowerBuilt += OnTowerBuilt;
                buildingTower = newTower;
            }
            else
            {
                Debug.Log($"Cannot buy tower {button_idx}, not enough gold");
            }
        }
        else if (buildingTower)
        {
            var towerGhost = buildingTower.GetComponent<BuildingTowerGhost>();
            if (towerGhost)
            {
                towerGhost.OnTowerBuilt -= OnTowerBuilt;
            }
            
            Destroy(buildingTower);
            buildingTower = null;
        }
    }

    private void OnTowerBuilt(TowerData obj)
    {
        Gold -= obj.goldCost;
        buildingTower = null;
    }
}
