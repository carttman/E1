using UnityEngine;

public class ProjectileTower : Tower
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shotsPerSecond;
    [SerializeField] private float damagePerShot = 25f;
    
    [SerializeField] private float launchProgress = 0f;

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
        var newProjectile = Instantiate(projectilePrefab, turret.position, Quaternion.identity);
        newProjectile.transform.LookAt(pTarget);
        newProjectile.transform.Rotate(Vector3.right, Random.Range(-10f, 10f));
        //newProjectile.transform.Rotate(Vector3.up, Random.Range(50f, 75f));
        
        var proj = newProjectile.GetComponent<Projectile>();
        proj.Target = pTarget;
        proj.Damage = damagePerShot;
        proj.instigator = this;
    }
}
