using UnityEngine;

public class ProjectileTower : Tower
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shotsPerSecond;
    [SerializeField] private float damagePerShot = 25f;
    
    [SerializeField] private float launchProgress = 0f;

    private new void Awake()
    {
        base.Awake();
        damagePerShot = towerData.damage;
        shotsPerSecond = towerData.attackSpeed;
    }
    
    private void Update()
    {
        launchProgress += shotsPerSecond * Time.deltaTime;
        while (launchProgress >= 1f)
        {
            if (AcquireTarget(out Transform pTarget))
            {
                if (pTarget)
                {
                    TrackTarget(ref pTarget);
                    Shoot(pTarget);
                }
                launchProgress -= 1f;
            }
            else
            {
                launchProgress = 0.999f;
            }
        }
    }

    private void Shoot(Transform pTarget)
    {
        var newProjectile = PoolManager.Instance.GetProjectile(projectilePrefab: projectilePrefab,
                                                                position: transform.position,
                                                                rotation: Quaternion.identity,
                                                                target: pTarget,
                                                                damage: damagePerShot,
                                                                instigator: this);
        newProjectile.transform.LookAt(pTarget);
        newProjectile.transform.Rotate(Vector3.right, Random.Range(-10f, 10f));
        newProjectile.gameObject.SetActive(true);
        //newProjectile.transform.Rotate(Vector3.up, Random.Range(50f, 75f));
    }
}
