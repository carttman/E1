using System;
using UnityEngine;

public class StatsComponent : MonoBehaviour
{
    public event Action Died;
    public event Action<float> HealthChanged;
    
    public float MaxHealth = 100f;
    [SerializeField]
    private float health;

    private void Awake()
    {
        health = MaxHealth;
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0) return;
        health -= damage;
        HealthChanged?.Invoke(health);
        if (health <= 0)
        {
            Died?.Invoke();
        }
    }
}
