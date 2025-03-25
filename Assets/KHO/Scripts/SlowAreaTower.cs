using UnityEngine;

public class SlowAreaTower : AreaTower
{
    [SerializeField] private float slowPercentage = 0.5f;
    [SerializeField] private float slowDuration = 3.5f;

    protected override void OnCollideDetected(Collider col)
    {
        var stats = col.GetComponent<StatsComponent>();
        if (!stats) return;
        stats.TakeDamage(damage, this);
        stats.GetComponent<EnemyState>()?.ApplySlow(slowPercentage, slowDuration);
    }
}
