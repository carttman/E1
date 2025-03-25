
using UnityEngine;
using UnityEngine.Serialization;
using RaycastHit = UnityEngine.RaycastHit;

public class Shell : MonoBehaviour
{
    [SerializeField] private Color blastColor = new Color(1f, 0.64f, 0f, 0.3f);
    
    private Vector3 launchPoint;
    private Vector3 targetPoint;
    private Vector3 launchVelocity;

    public Tower instigator;
    
    public float blastRadius = 5f;
    public float damage = 100f;

    private float age;

    public void Initialize(Vector3 launchPoint, Vector3 targetPoint, Vector3 launchVelocity, Tower instigator)
    {
        this.launchPoint = launchPoint;
        this.targetPoint = targetPoint;
        this.launchVelocity = launchVelocity;
        this.instigator = instigator;
    }

    private void Update()
    {
        age += Time.deltaTime;
        Vector3 p = launchPoint + launchVelocity * age;
        p.y -= 0.5f * 9.81f * age * age;
        transform.localPosition = p;

        if (p.y < 0f)
        {
            var explosionGameObject = Instantiate(Game.Instance.explosionPrefab, transform.position, Quaternion.identity);
            var explosion = explosionGameObject.GetComponent<Explosion>();
            explosion.Initialize(0.5f, blastRadius, blastColor,false);
            explosion.OnCollideDetected += OnCollideDetected;
            
            Destroy(gameObject);
        }

        Vector3 d = launchVelocity;
        d.y -= 9.81f * age;
        transform.localRotation = Quaternion.LookRotation(d);
    }

    private void OnCollideDetected(Collider col)
    {
        var stats = col.GetComponent<StatsComponent>();
        if (!stats) return;
        stats.TakeDamage(damage, instigator);
    }
}
