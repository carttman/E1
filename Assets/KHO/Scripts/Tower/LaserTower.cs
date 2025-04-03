using UnityEngine;

public class LaserTower : Tower
{
    private const float LaserBeamVFXlength = 12f;
    [SerializeField] private Transform laserBeam;
    [SerializeField] protected float damagePerSecond = 10f;
    private Vector3 _laserBeamScale = Vector3.one;
    private Vector3 _laserBeamStartPos;

    private new void Awake()
    {
        base.Awake();
        OnRarityChanged();
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
            AudioManager.instance.PlaySound(SoundEffect.LaserBeam);
            Shoot(target);
        }
        else
            laserBeam.gameObject.SetActive(false);
    }

    protected override void OnRarityChanged()
    {
        damagePerSecond = towerData.TowerStats[(int)Rarity].damage;
    }

    private void Shoot(Transform target)
    {
        var position = target.position;
        laserBeam.gameObject.SetActive(true);
        laserBeam.LookAt(position);

        var dist = Vector3.Distance(laserBeam.position, position);
        _laserBeamScale.z = dist / LaserBeamVFXlength;
        laserBeam.localScale = _laserBeamScale;

        var sc = target.GetComponent<StatsComponent>();
        if (sc)
        {
            var damagePacket = new DamagePacket(damagePerSecond * Time.deltaTime, towerData.elementType, this);
            sc.TakeDamage(damagePacket, true);
        }
    }
}