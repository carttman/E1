using UnityEngine;

public class Enemy : MonoBehaviour
{
    private StatsComponent _statsComponent;
    
    private void Awake()
    {
        _statsComponent = GetComponent<StatsComponent>();
        if (_statsComponent != null)
        {
            _statsComponent.Died += () => Destroy(gameObject);
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
        WaveSpawner.EnemiesAlive--;
        Destroy(gameObject);
    }
}
