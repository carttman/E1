using UnityEngine;
using UnityEngine.Serialization;

public class Shell : Projectile
{
    
    public float blastRadius = 5f;
    public float damage = 100f;
    
    [SerializeField] private Color blastColor = new Color(1f, 0.64f, 0f, 0.3f);
    [SerializeField] private bool initialized = false;
    
    private Vector3 _launchPoint;
    private Vector3 _targetPoint;
    private Vector3 _launchVelocity;
    
    public void Initialize(Vector3 launchPoint, Vector3 targetPoint, Vector3 launchVelocity, float inDamage, Tower inInstigator)
    {
        this._launchPoint = launchPoint;
        this._targetPoint = targetPoint;
        this._launchVelocity = launchVelocity;
        this.instigator = inInstigator;
        this.damage = inDamage;
        
        initialized = true;
    }

    protected override void OnUpdate()
    {
        if (!initialized) return;
        
        Vector3 p = _launchPoint + _launchVelocity * age;
        p.y -= 0.5f * 9.81f * age * age;
        transform.position = p;

        if (p.y < 0f)
        {
            var explosionGameObject = Instantiate(Game.Instance.explosionPrefab, transform.position, Quaternion.identity);
            var explosion = explosionGameObject.GetComponent<Explosion>();
            explosion.Initialize(0.5f, blastRadius, blastColor,false);
            explosion.OnCollideDetected += OnCollideDetected;
            
            Destroy(gameObject);
        }

        Vector3 d = _launchVelocity;
        d.y -= 9.81f * age;
        transform.localRotation = Quaternion.LookRotation(d);
    }

    private void OnCollideDetected(Collider col)
    {
        var stats = col.GetComponent<StatsComponent>();
        if (!stats) return;
        stats.TakeDamage(damage, instigator);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        initialized = false;
    }
}
