using UnityEngine;

public class Shell : Projectile
{
    public float blastRadius = 5f;
    public float damage = 100f;

    [SerializeField] private Color blastColor = new(1f, 0.64f, 0f, 0.3f);
    [SerializeField] private bool initialized;

    private Vector3 _launchPoint;
    private Vector3 _launchVelocity;
    private Vector3 _targetPoint;

    protected override void OnDisable()
    {
        base.OnDisable();
        initialized = false;
    }

    public void Initialize(Vector3 launchPoint, Vector3 targetPoint, Vector3 launchVelocity)
    {
        _launchPoint = launchPoint;
        _targetPoint = targetPoint;
        _launchVelocity = launchVelocity;

        initialized = true;
    }

    protected override void OnUpdate()
    {
        if (!initialized) return;

        var p = _launchPoint + _launchVelocity * age;
        p.y -= 0.5f * 9.81f * age * age;
        transform.position = p;

        if (p.y < 0f)
        {
            var explosionGameObject =
                Instantiate(Game.Instance.explosionPrefab, transform.position, Quaternion.identity);
            var explosion = explosionGameObject.GetComponent<Explosion>();
            explosion.Initialize(0.5f, blastRadius, blastColor, false);
            var damagePacketCapture = DamagePacket;
            explosion.OnCollideDetected += col => OnCollideDetected(col, damagePacketCapture);

            Release();
        }

        var d = _launchVelocity;
        d.y -= 9.81f * age;
        transform.localRotation = Quaternion.LookRotation(d);
    }

    private void OnCollideDetected(Collider col, DamagePacket damagePacket)
    {
        var stats = col.GetComponent<StatsComponent>();
        if (!stats) return;
        stats.TakeDamage(damagePacket);
    }
}