using UnityEngine;

public class ProjectileTower : Tower
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private float shotsPerSecond;
    [SerializeField] private float damagePerShot = 25f;

    [SerializeField] private float launchProgress;
    [SerializeField] private SoundEffect soundEffect;

    private new void Start()
    {
        base.Start();
        OnRarityChanged();
    }

    private new void Update()
    {
        base.Update();
        
        launchProgress += shotsPerSecond * Time.deltaTime;
        while (launchProgress >= 1f)
            if (AcquireTarget(out var pTarget))
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

    protected override void OnRarityChanged()
    {
        damagePerShot = towerData.TowerStats[(int)Rarity].damage;
        shotsPerSecond = towerData.TowerStats[(int)Rarity].attackSpeed;
    }

    private void Shoot(Transform pTarget)
    {
        var damagePacket = new DamagePacket(damagePerShot, towerData.elementType, this);
        var newProjectile = PoolManager.Instance.GetProjectile(projectilePrefab,
            turret.position,
            Quaternion.identity,
            pTarget,
            damagePacket);
        newProjectile.transform.LookAt(pTarget);
        newProjectile.transform.Rotate(Vector3.right, Random.Range(-10f, 10f));
        newProjectile.gameObject.SetActive(true);
        //newProjectile.transform.Rotate(Vector3.up, Random.Range(50f, 75f));
        
        AudioManager.instance.PlaySound(soundEffect);
    }
}