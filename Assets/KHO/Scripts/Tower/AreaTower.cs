using UnityEngine;

public class AreaTower : Tower
{
    [SerializeField] public GameObject attackAreaEffectPrefab;
    [SerializeField] public float attacksPerSecond;
    [SerializeField] private float damage;
    private float _attackProgress = 0f;
    
    private void Update()
    {
        _attackProgress += attacksPerSecond * Time.deltaTime;
        while (_attackProgress >= 1f)
        {
            if (AcquireTarget(out Transform pTarget))
            {
                if (pTarget)
                {
                    Attack();
                }
                _attackProgress -= 1f;
            }
            else
            {
                _attackProgress = 0.999f;
            }
        }
    }

    private void Attack()
    {
        Instantiate(attackAreaEffectPrefab, transform.position, Quaternion.identity);
        foreach (var enemy in potentialTargets)
        {
            enemy?.GetComponent<StatsComponent>()?.TakeDamage(damage, this);
        }
    }
}
