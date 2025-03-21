using System;
using UnityEngine;

public class Enemy : MonoBehaviour, ISelectable
{
    public delegate void EnemyDiedEventHandler(Enemy enemy, float goldDropAmount);
    public delegate void EnemyEndPathEventHandler(Enemy enemy, int livesDamage);

    // 사망시 호출되는 이벤트
    public event EnemyDiedEventHandler OnEnemyDied;
    // 적이 끝까지 도달시 호출되는 이벤트
    public event EnemyEndPathEventHandler OnEnemyEndPath;
    
    
    // 적 사망시 드랍되는 골드
    [SerializeField] private float goldDropAmount = 1;
    public Transform PopupTransform;

    // 적이 끝까지 도달시 목숨 데미지
    [SerializeField] private int livesDamage = 1;

    // 스탯처리 컴포넌트
    private StatsComponent _statsComponent;
    
    // 선택시 표시
    [SerializeField] private GameObject _selectionIndicator;

    // 시간
    public float Age;
    
    private void Awake()
    {
        _statsComponent = GetComponent<StatsComponent>();
        if (_statsComponent != null)
        {
            _statsComponent.Died += Die;
        }

        _enemySelectionData = new EnemySelectionData
        {
            Name = gameObject.name,
            Health = _statsComponent.MaxHealth,
            MaxHealth = _statsComponent.MaxHealth,
            MoveSpeed = GetComponent<EnemyMove>()?.speed ?? 0f
        };
        
        _statsComponent.HealthChanged += newHealth =>
        {
            _enemySelectionData.Health = newHealth;
            _enemySelectionData.MaxHealth = _statsComponent.MaxHealth;
        };

        _enemySelectionData.OnSelectionDataChanged += data => OnSelectionDataChanged?.Invoke(data);
    }

    private void Update()
    {
        Age += Time.deltaTime;
    }

    private void Die()
    {
        OnEnemyDied?.Invoke(this, goldDropAmount);
        PopUpManager.Instance.PopUpUI("+" + goldDropAmount.ToString(), PopupTransform.position, Color.yellow, 2);
        WaveSpawner.EnemiesAlive--;
        Destroy(gameObject);
    }

    public void EndPath()
    {
        OnEnemyEndPath?.Invoke(this, livesDamage);
        WaveSpawner.EnemiesAlive--;
        Destroy(gameObject); //도착지점 도착 시 오브젝트 파괴
    }

    // Start of ISelectable
    private EnemySelectionData _enemySelectionData;
    
    public event Action<SelectionData> OnSelectionDataChanged;

    public void OnSelect()
    {
        if (_selectionIndicator)
        {
            _selectionIndicator.SetActive(true);
        }
    }

    public void OnDeselect()
    {
        if (_selectionIndicator)
        {
            _selectionIndicator.SetActive(false);
        }
    }
    public SelectionData GetSelectionData() => _enemySelectionData;
    // End of ISelectable

    private void OnMouseUpAsButton()
    {
        OnSelect();
        SelectionManager.instance.OnSelect(this);
    }
}
