using System;
using UnityEngine;

public class LaserTower : Tower
{
    [SerializeField] private Transform laserBeam;
    [SerializeField] protected float damagePerSecond = 10f;

    private Vector3 _laserBeamScale = Vector3.one;
    private Vector3 _laserBeamStartPos;
    
    private new void Awake()
    {
        base.Awake();
        OnRarityChanged();
    }

    protected override void OnRarityChanged()
    {
        damagePerSecond = towerData.TowerStats[(int)Rarity].damage;
    }

    private new void Start()
    {
        base.Start();
        _laserBeamScale = laserBeam.localScale;
        _laserBeamStartPos = laserBeam.localPosition;
    }

    private void Update()
    {
        if (AcquireTarget(out var target))
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
        var position = target.position;
        laserBeam.LookAt(position);
        
        var dist = Vector3.Distance(laserBeam.position, position);
        _laserBeamScale.z = dist;
        laserBeam.localScale = _laserBeamScale;
        laserBeam.localPosition = _laserBeamStartPos + laserBeam.forward * (dist * 0.5f);
    
        var sc = target.GetComponent<StatsComponent>();
        if (sc)
        {
            var damagePacket = new DamagePacket(damagePerSecond * Time.deltaTime, towerData.elementType, this);
            sc.TakeDamage(damagePacket, true);
        }
    }
}
