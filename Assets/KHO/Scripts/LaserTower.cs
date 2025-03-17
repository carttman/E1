using UnityEngine;

public class LaserTower : Tower
{
    public float cooldown = 1f;
    public float currentDuration = 0f;

    [SerializeField] private Transform laserBeam;
    [SerializeField] private Vector3 _laserBeamScale = Vector3.one;

    private void Awake()
    {
        _laserBeamScale = laserBeam.localScale;
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.localPosition;
        position.y += 0.01f;
        Gizmos.DrawWireSphere(position, targetingRange);
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
            //laserBeam.localScale = Vector3.zero;
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
            sc.TakeDamage(damagePerSecond * 1f * Time.deltaTime);
        }
    }
}
