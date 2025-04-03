using UnityEngine;

public class AreaTower : Tower
{
    [SerializeField] protected float attacksPerSecond;
    [SerializeField] protected float damage;
    [SerializeField] protected float blastRadius = int.MinValue;
    [SerializeField] protected Color blastColor = new(1, 0, 0, 0.3f);

    protected float AttackProgress = 0.999f;

    private new void Awake()
    {
        base.Awake();
    }

    protected new void Start()
    {
        base.Start();
        OnRarityChanged();
    }

    protected void Update()
    {
        AttackProgress += attacksPerSecond * Time.deltaTime;
        while (AttackProgress >= 1f)
            if (AcquireTarget(out var pTarget))
            {
                if (pTarget) Attack();
                AttackProgress -= 1f;
            }
            else
            {
                AttackProgress = 0.999f;
            }
    }

    protected override void OnRarityChanged()
    {
        // blast radius가 특정되지 않을경우 targetingRange 변수 그대로 사용
        if (Mathf.Approximately(blastRadius, int.MinValue)) blastRadius = targetingRange;
        damage = towerData.TowerStats[(int)Rarity].damage;
        attacksPerSecond = towerData.TowerStats[(int)Rarity].attackSpeed;
    }

    protected void Attack()
    {
        var explosionGameObject = Instantiate(Game.Instance.explosionPrefab, transform.position, Quaternion.identity);
        var explosion = explosionGameObject.GetComponent<Explosion>();
        explosion.Initialize(0.5f, blastRadius, blastColor, false);
        explosion.OnCollideDetected += OnCollideDetected;
        
        AudioManager.instance.PlaySound(SoundEffect.AreaAttack);
    }

    protected virtual void OnCollideDetected(Collider col)
    {
        var stats = col.GetComponent<StatsComponent>();
        if (!stats) return;

        var damagePacket = new DamagePacket(damage, towerData.elementType, this);
        stats.TakeDamage(damagePacket);
    }
}