using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class HomingProjectile : Projectile
{
    private Rigidbody _rb;
    private EnemyMove _enemyMove;
    private SphereCollider _collider;

    [SerializeField] private float maxLifetime = 10f;
    
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

    [SerializeField, Range(0f, 1f)] private float hitChance = 0.5f;
    
    private Vector3 _standardPrediction;
    private Vector3 _deviatedPrediction;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        _enemyMove = Target.GetComponent<EnemyMove>();
        _collider.radius = _triggerRadius;
        Destroy(gameObject, maxLifetime);
    }

    private void FixedUpdate()
    {
        _rb.linearVelocity = transform.forward * speed;

        if (!Target) return;
        
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
        _standardPrediction = _enemyMove?.GetPredictedPosition(predictionTime) ?? Target.position;
    }

    private void AddDeviation(float leadTimePercentage)
    {
        var deviation = new Vector3(Mathf.Cos(Time.time * _deviationSpeed), 0, 0);
        var predictionOffset = transform.TransformDirection(deviation) * (_deviationAmount * leadTimePercentage);
        _deviatedPrediction = _standardPrediction + predictionOffset;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Random.Range(0f, 1f) > hitChance) return;
        var enemy = other.GetComponent<Enemy>();
        Debug.Assert(enemy);

        enemy.GetComponent<StatsComponent>().TakeDamage(Damage, instigator);
        Destroy(gameObject);
    }

    protected override void OnUpdate() { }

}
