using UnityEngine;
using Random = UnityEngine.Random;

public class HomingProjectile : Projectile
{
    
    [Header("EXPLOSION")]
    public bool isAoe = false;
    public float aoeRadius = 3f;
    [SerializeField] private Color explosionColor = new Color(1, 1, 0, 0.3f);
    
    [Header("HOMING PROJECTILE")]
    [SerializeField] private float _rotateSpeed = 90f;
    [Space]
    [SerializeField] private float _minDistancePredict = 5f;
    [SerializeField] private float _maxDistancePredict = 100f;
    [SerializeField] private float _maxTimePrediction = 3f;
    [Space]
    [SerializeField] private float _deviationSpeed = 2f;
    [SerializeField] private float _deviationAmount = 50f;
    [Space]
    [SerializeField] private float _triggerRadius = 0.25f;
    [SerializeField] private float maxLifetime = 10f;
    [SerializeField, Range(0f, 1f)] private float hitChance = 0.5f;
    
    private Vector3 _standardPrediction;
    private Vector3 _deviatedPrediction;
    
    private Rigidbody _rb;
    private SphereCollider _collider;
    private EnemyMove _enemyMove;

    private bool _collided = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<SphereCollider>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        _collider.radius = _triggerRadius;
        _enemyMove = Target.GetComponent<EnemyMove>();
    }

    private void FixedUpdate()
    {
        if (age >= maxLifetime)
        {
            if (_collided) return;
            Release();
            enabled = false;
            return;
        }
        
        _rb.linearVelocity = transform.forward * speed;

        if (!Target) return;
        if (!_enemyMove) return;
        
        float leadTimePercentage = Mathf.InverseLerp(_minDistancePredict, _maxDistancePredict,
            Vector3.Distance(transform.position, Target.transform.position));
        PredictMovement(leadTimePercentage);
        AddDeviation(leadTimePercentage);
        RotateRocket();
    }

    private void RotateRocket()
    {
        var heading = _deviatedPrediction - transform.position;
        var rotation = Quaternion.LookRotation(heading);
        _rb.MoveRotation(Quaternion.RotateTowards(transform.rotation, rotation, _rotateSpeed * Time.deltaTime));
    }

    private void PredictMovement(float leadTimePercentage)
    {
        var predictionTime = Mathf.Lerp(0, _maxTimePrediction, leadTimePercentage);
        _standardPrediction = _enemyMove?.GetPredictedPosition(predictionTime) ?? Target?.position ?? Vector3.zero;
    }

    private void AddDeviation(float leadTimePercentage)
    {
        var deviation = new Vector3(Mathf.Cos(Time.time * _deviationSpeed), 0, 0);
        var predictionOffset = transform.TransformDirection(deviation) * (_deviationAmount * leadTimePercentage);
        _deviatedPrediction = _standardPrediction + predictionOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_collided) return;
        if (Random.Range(0f, 1f) > hitChance) return;

        _collided = true;
        var enemy = other.GetComponent<Enemy>();
        if (isAoe)
        {
            var explosionGO = Instantiate(Game.Instance.explosionPrefab, transform.position, Quaternion.identity);
            var explosion = explosionGO.GetComponent<Explosion>();
            explosion.GetComponent<Explosion>().Initialize(0.5f, aoeRadius, new Color(1, 1, 0, 0.3f), false);
            explosion.OnCollideDetected += col => col.GetComponent<StatsComponent>()?.TakeDamage(Damage, instigator);
        }
        else
        {
            enemy.GetComponent<StatsComponent>().TakeDamage(Damage, instigator);
        }
        Release();
        enabled = false;
    }

    protected override void OnUpdate() { }
}
