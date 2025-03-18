using UnityEngine;

public class LaserTower : Tower
{
    [SerializeField] private Transform laserBeam;
    [SerializeField] private Vector3 _laserBeamScale = Vector3.one;
    [SerializeField] protected float damagePerSecond = 10f;
    
    private new void Awake()
    {
        base.Awake();
        _laserBeamScale = laserBeam.localScale;
    }

    private void Update()
    {
        Transform target;
        if (AcquireTarget(out target))
        {
            if (TrackTarget(ref target))
            {
                Shoot(target);
            }
        }
        else
        {
            laserBeam.localScale = Vector3.zero;
        }
    }

    private void Shoot(Transform target)
    {
        Vector3 position = target.position;
        laserBeam.localRotation = turret.localRotation;
        
        float dist = Vector3.Distance(turret.position, position);
        _laserBeamScale.z = dist;
        laserBeam.localScale = _laserBeamScale;
        laserBeam.localPosition = turret.localPosition + laserBeam.forward * (dist * 0.5f);
    
        StatsComponent sc = target.GetComponent<StatsComponent>();
        if (sc)
        {
            sc.TakeDamage(damagePerSecond * Time.deltaTime);
        }
    }
}
