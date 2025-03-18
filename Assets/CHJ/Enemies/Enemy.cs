using System;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public delegate void EnemyDiedEventHandler(Enemy enemy, float goldDropAmount);
    public event EnemyDiedEventHandler EnemyDied;
    
    private StatsComponent _statsComponent;
    
    [SerializeField]
    private float goldDropAmount = 1;
    
    private void Awake()
    {
        _statsComponent = GetComponent<StatsComponent>();
        if (_statsComponent != null)
        {
            _statsComponent.Died += Die;
        }
    }
    
    private void Start()
    {
       
    }

    void Update()
    {
       
    }

    void GetNextWayPoint()
    {
       
    }

    void Die()
    {
        EnemyDied?.Invoke(this, goldDropAmount);
        WaveSpawner.EnemiesAlive--;
        Destroy(gameObject);
    }
}
