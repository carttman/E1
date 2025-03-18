using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void EnemyDiedEventHandler(Enemy enemy, float goldDropAmount);

    public delegate void EnemyEndPathEventHadnler(Enemy enemy, int livesDamage);

    public event EnemyDiedEventHandler OnEnemyDied;
    public event EnemyEndPathEventHadnler OnEnemyEndPath;
    
    private StatsComponent _statsComponent;

    [SerializeField] private float goldDropAmount = 1;
    [SerializeField] private int livesDamage = 1;

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
