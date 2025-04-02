using UnityEngine;

public class SlowAreaTower : AreaTower
{
    [SerializeField] private float slowPercentage = 0.5f;
    [SerializeField] private float slowDuration = 3.5f;

    protected override void OnCollideDetected(Collider col)
    {
        var stats = col.GetComponent<StatsComponent>();
        if (!stats) return;

        // Apply damage and slow effect
        var damagePacket = new DamagePacket(damage, towerData.elementType, this);
        stats.TakeDamage(damagePacket);
        stats.GetComponent<EnemyState>()?.ApplySlow(slowPercentage, slowDuration);
    }
}