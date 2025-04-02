using System;
using UnityEngine;

// HP 등 스탯 처리하는 컴포넌트
public class StatsComponent : MonoBehaviour
{
    [SerializeField] public Global.Element element;

    public float maxHealth = 100f;

    [SerializeField] private float health;

    public Transform PopupTransform;
    public float Health => health;

    private void Awake()
    {
        health = maxHealth;
    }

    public event Action Died;
    public event Action<float> HealthChanged;

    public void TakeDamage(DamagePacket damagePacket, bool shouldAccumulate = false)
    {
        if (damagePacket.Value <= 0 || health <= 0) return;

        // weakness = 1, resistance = -1, normal = 0
        var weakness = damagePacket.ElementType.WinsTo(element) ? 1 :
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
        if (instigator) instigator.DealtDamage += finalDamage;

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
                1 => 1.2f, // Weakness
                -1 => 0.92f, // Resistance
                _ => 1f // Normal
            };

            var weaknessText = weakness switch
            {
                0 => "",
                _ => $" ({multiplier}x)"
            };

            // 데미지 팝업 호출
            PopUpManager.Instance.CreatePopUpUI($"{finalDamage}{weaknessText}", PopupTransform.position, popupColor,
                size);
        }


        HealthChanged?.Invoke(health);
        if (health <= 0)
        {
            if (instigator) instigator.Kills++;
            Died?.Invoke();
        }
    }
}