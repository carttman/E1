using Unity.VisualScripting;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public enum TowerAttackType
    {
        Constant,
        Cooldown
    }

    public TowerAttackType attackType = TowerAttackType.Cooldown;
    public float cooldown = 1f;
    public float currentDuration = 0f;
    
    [SerializeField, Range(1.5f, 10.5f)]
    private float targetingRange = 1.5f;

    public float TargetingRange
    {
        get => targetingRange;
        set
        {
            if (Mathf.Approximately(targetingRange, value)) return;
            targetingRange = value;
            _sphereCollider.radius = targetingRange;
        }
    }

    [SerializeField] private Transform target;
    [SerializeField] private Transform rotatingPart;
    [SerializeField] private Transform turret;
    [SerializeField] private Transform laserBeam;

    [SerializeField] private float DamagePerSecond = 10f;

    [SerializeField] public TowerData TowerData;
    [SerializeField] private GameObject projectilePrefab;
    
    private SphereCollider _sphereCollider;
    private Vector3 _laserBeamScale;
    
    private void Awake()
    {
        Debug.Assert(TowerData);
        
        _sphereCollider = GetComponent<SphereCollider>();
        _sphereCollider.radius = targetingRange;
        _laserBeamScale = laserBeam.localScale;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.localPosition;
        position.y += 0.01f;
        Gizmos.DrawWireSphere(position, targetingRange);
    }

    private void OnValidate()
    {
        TargetingRange = targetingRange;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"{other.name} has entered the tower range");
        if (other.CompareTag("Enemy"))
        {
            target = other.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == target)
        {
            target = null;
            Debug.Log($"{other.name} has exited the tower range");
        }
    }

    private void Update()
    {
        if (target)
        {
            Shoot();
        }
        else
        {
            laserBeam.localScale = Vector3.zero;
        }
    }

    private void Shoot()
    {
        Vector3 position = target.position;
        turret.LookAt(position);
        laserBeam.localRotation = turret.localRotation;

        float dist = Vector3.Distance(turret.position, position);
        _laserBeamScale.z = dist;
        laserBeam.localScale = _laserBeamScale;
        laserBeam.localPosition = turret.localPosition + laserBeam.forward * (dist * 0.5f);

        if (attackType == TowerAttackType.Constant)
        {
            StatsComponent sc = target.GetComponent<StatsComponent>();
            if (sc)
            {
                sc.TakeDamage(DamagePerSecond * 1f * Time.deltaTime);
            }
        }
        else if (attackType == TowerAttackType.Cooldown)
        {
            currentDuration += Time.deltaTime;
            if (currentDuration >= cooldown)
            {
                currentDuration -= cooldown;
                CooldownAttack(target);
            }
        }
    }

    private void CooldownAttack(Transform _target)
    {
        StatsComponent statsComponent = _target.GetComponent<StatsComponent>();
        if (!statsComponent) return;
        
        // statsComponent.TakeDamage(DamagePerSecond / cooldown * 1f);
        // Debug.Log($"{statsComponent.name} has been taken damage for {DamagePerSecond / cooldown * 1f}");
        var p = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        p.GetComponent<Projectile>().target = _target;
        p.GetComponent<Projectile>().damage = DamagePerSecond / cooldown * 1f;
    }
}
