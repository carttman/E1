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
    public float Health { get; }
    
    //PopUpManager PopUp;
    public Transform PopupTransform;
    
    private void Awake()
    {
        health = MaxHealth;
    }

    
    
    
    private void Update()
    {
        
    }

    public void TakeDamage(float damage, Tower instigator = null)
    {
        if (damage <= 0 || health <= 0) return;
        health -= damage;
        
        // 데미지 팝업 호출
        PopUpManager.Instance.PopUpUI(damage.ToString(), PopupTransform.position, Color.red, 1);
        
        HealthChanged?.Invoke(health);
        if (health <= 0)
        {
            if (instigator != null)
            {
                instigator.Kills++;
            }
            Died?.Invoke();
        }
    }
}
