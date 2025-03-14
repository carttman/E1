using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private StatsComponent statsComponent;

    private void Awake()
    {
        Debug.Assert(statsComponent, $"No stats component assigned for health bar {name}");
        Debug.Assert(slider, $"No slider assigned for health bar {name}");
        statsComponent.HealthChanged += OnHealthChanged;
    }

    private void OnHealthChanged(float newHealth)
    {
        slider.value = newHealth / statsComponent.MaxHealth;
    }
}
