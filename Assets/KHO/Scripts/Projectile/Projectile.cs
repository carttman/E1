using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public ProjectilePool pool;
    public Transform target;
    public DamagePacket DamagePacket;
    
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float age = 0f;
    [SerializeField] protected TrailRenderer[] trail;

    private void Awake()
    {
        trail = transform.GetComponentsInChildren<TrailRenderer>();
    }

    protected virtual void OnEnable()
    {
        if (trail != null)
        {
            foreach (var t in trail)
            {
                t.Clear();
            }
        }
    }
    
    private void Update()
    {
        age += Time.deltaTime;
        OnUpdate();
    }

    public void Release()
    {
        pool?.Release(this);
    }
    
    protected virtual void OnDisable()
    {
        age = 0f;
        DamagePacket = new DamagePacket();
        target = null;
    }

    protected abstract void OnUpdate();
}
