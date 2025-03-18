using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

// HP 등 스탯 처리하는 컴포넌트
public class StatsComponent : MonoBehaviour
{
    public event Action Died;
    public event Action<float> HealthChanged;
    
    public float MaxHealth = 100f;
    [SerializeField]
    private float health;
    
    [Header("Unity Stuff")]
    public Image healthBar;
    public Camera MyCamera;
    public Canvas HPCanvas;
    
    private void Awake()
    {
        health = MaxHealth;
        
        MyCamera = Camera.main;
        HPCanvas.worldCamera = MyCamera;
    }

    
    
    
    private void Update()
    {
        Vector3 dir = MyCamera.transform.position - HPCanvas.transform.position;
        Vector3 newVec = new Vector3(dir.x, dir.y, 0);
        HPCanvas.transform.rotation = Quaternion.LookRotation(newVec.normalized);
    }

    public void TakeDamage(float damage)
    {
        if (damage <= 0) return;
        health -= damage;
        
        healthBar.fillAmount = health / MaxHealth;
        
        HealthChanged?.Invoke(health);
        if (health <= 0)
        {
            Died?.Invoke();
        }
    }
}
