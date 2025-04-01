using System;
using System.Globalization;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

// HP 등 스탯 처리하는 컴포넌트
public class StatsComponent : MonoBehaviour
{
    public event Action Died;
    public event Action<float> HealthChanged;

    [SerializeField] public Global.Element element;
    
    public float maxHealth = 100f;
    [SerializeField]
    private float health;
    public float Health
    {
        get => health;
    }
    
    public Transform PopupTransform;
    
    private void Awake()
    {
        health = maxHealth;
    }
    
    public void TakeDamage(DamagePacket damagePacket, bool shouldAccumulate = false)
    {
        if (damagePacket.Value <= 0 || health <= 0) return;
        
        // weakness = 1, resistance = -1, normal = 0
        int weakness = damagePacket.ElementType.WinsTo(element) ? 1 :
                       damagePacket.ElementType.LosesTo(element) ? -1 : 0;

        var globalData = Game.Instance.GlobalData;
        
        var multiplier = weakness switch
        {
            1 => globalData.ElementWeaknessMultiplier,
            -1 => globalData.ElementResistanceMultiplier,
            _ => 1f
        };
        
        var finalDamage = multiplier * damagePacket.Value;

        if (finalDamage <= 0) return;
        health -= finalDamage;

        var instigator = damagePacket.Instigator;
        if (instigator)
        {
            instigator.DealtDamage += finalDamage;
        }
        
        if (!shouldAccumulate)
        {
            var popupColor = weakness switch
            {
                1 => globalData.WeaknessDamageColor, // Weakness
                -1 => globalData.ResistanceDamageColor, // Resistance
                _ => globalData.NormalDamageColor // Normal
            };

            var size = weakness switch
            {
                1 => 1.5f, // Weakness
                -1 => 0.5f, // Resistance
                _ => 1f // Normal
            };
            
            // 데미지 팝업 호출
            PopUpManager.Instance.CreatePopUpUI(finalDamage.ToString(CultureInfo.CurrentCulture), PopupTransform.position, popupColor, size);
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
