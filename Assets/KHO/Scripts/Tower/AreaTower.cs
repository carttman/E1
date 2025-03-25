using UnityEngine;

public class AreaTower : Tower
{
    [SerializeField] protected float attacksPerSecond;
    [SerializeField] protected float damage;
    [SerializeField] protected float blastRadius = int.MinValue;
    [SerializeField] protected Color blastColor = new Color(1, 0, 0, 0.3f);
    
    protected float AttackProgress = 0f;
    
    protected new void Start()
    {
        base.Start();
        // 기본적으로 폭발 범위 = 타겟 범위로 사용
        if (Mathf.Approximately(blastRadius, int.MinValue))
        {
            blastRadius = targetingRange;
        }
    }
    
    protected void Update()
    {
        AttackProgress += attacksPerSecond * Time.deltaTime;
        while (AttackProgress >= 1f)
        {
            if (AcquireTarget(out Transform pTarget))
            {
                if (pTarget)
                {
                    Attack();
                }
                AttackProgress -= 1f;
            }
            else
            {
                AttackProgress = 0.999f;
            }
        }
    }

    protected void Attack()
    {
        var explosionGameObject = Instantiate(Game.Instance.explosionPrefab, transform.position, Quaternion.identity);
        var explosion = explosionGameObject.GetComponent<Explosion>();
        explosion.Initialize(0.5f, blastRadius, blastColor, false);
        explosion.OnCollideDetected += OnCollideDetected;
    }

    protected virtual void OnCollideDetected(Collider col)
    {
        var stats = col.GetComponent<StatsComponent>();
        if (!stats) return;
        stats.TakeDamage(damage, this);
    }
}
