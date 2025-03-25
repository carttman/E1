using System;
using UnityEngine;

public class LaserTower : Tower
{
    [SerializeField] private Transform laserBeam;
    [SerializeField] protected float damagePerSecond = 10f;

    private Vector3 _laserBeamScale = Vector3.one;

    private Transform _target;

    private new void Start()
    {
        _laserBeamScale = laserBeam.localScale;
    }

    private void Update()
    {
        Transform target;
        if (AcquireTarget(out target))
        {
            TrackTarget(ref target);
            Shoot(target);
        }
        else
        {
            laserBeam.localScale = Vector3.zero;
        }
    }

    private void Shoot(Transform target)
    {

        _target = target;
        
        Vector3 position = target.position;
        laserBeam.LookAt(position);
        
        float dist = Vector3.Distance(laserBeam.position, position);
        _laserBeamScale.z = dist;
        laserBeam.localScale = _laserBeamScale;
        laserBeam.localPosition = turret.localPosition + laserBeam.forward * (dist * 0.6f);
    
        StatsComponent sc = target.GetComponent<StatsComponent>();
        if (sc)
        {
            sc.TakeDamage(damagePerSecond * Time.deltaTime, this, true);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_target.position, 0.5f);
    }
}
