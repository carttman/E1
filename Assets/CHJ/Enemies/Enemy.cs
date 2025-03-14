using UnityEngine;

public class Enemy : MonoBehaviour
{
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
