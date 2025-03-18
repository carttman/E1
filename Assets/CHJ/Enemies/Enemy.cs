using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void EnemyDiedEventHandler(Enemy enemy, float goldDropAmount);
    public delegate void EnemyEndPathEventHandler(Enemy enemy, int livesDamage);

    // 사망시 호출되는 이벤트
    public event EnemyDiedEventHandler OnEnemyDied;
    // 적이 끝까지 도달시 호출되는 이벤트
    public event EnemyEndPathEventHandler OnEnemyEndPath;
    
    
    // 적 사망시 드랍되는 골드
    [SerializeField] private float goldDropAmount = 1;
    // 적이 끝까지 도달시 목숨 데미지
    [SerializeField] private int livesDamage = 1;
    

    // 스탯처리 컴포넌트
    private StatsComponent _statsComponent;

    private void Awake()
    {
        _statsComponent = GetComponent<StatsComponent>();
        if (_statsComponent != null)
        {
            _statsComponent.Died += Die;
        }
    }

    private void Die()
    {
        OnEnemyDied?.Invoke(this, goldDropAmount);
        WaveSpawner.EnemiesAlive--;
        Destroy(gameObject);
    }

    public void EndPath()
    {
        OnEnemyEndPath?.Invoke(this, livesDamage);
        WaveSpawner.EnemiesAlive--;
        Destroy(gameObject); //도착지점 도착 시 오브젝트 파괴
    }
}
