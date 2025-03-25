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
    public float Health
    {
        get => health;
    }
    
    //PopUpManager PopUp;
    public Transform PopupTransform;
    
    private void Awake()
    {
        health = MaxHealth;
    }
    
    public void TakeDamage(float damage, Tower instigator = null, bool shouldAccumulate = false)
    {
        if (damage <= 0 || health <= 0) return;
        health -= damage;

        if (instigator)
        {
            instigator.DealtDamage += damage;
        }

        if (!shouldAccumulate)
        {
            // 데미지 팝업 호출
            PopUpManager.Instance.CreatePopUpUI(damage.ToString(), PopupTransform.position, Color.red, 1);
        }
        
        
        HealthChanged?.Invoke(health);
        if (health <= 0)
        {
            if (instigator)
            {
                instigator.Kills++;
            }
            Died?.Invoke();
        }
    }
}
