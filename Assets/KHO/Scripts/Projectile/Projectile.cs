using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    public ProjectilePool pool;
    public Transform target;
    public DamagePacket DamagePacket;

    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float age;
    [SerializeField] protected TrailRenderer[] trail;

    private void Awake()
    {
        trail = transform.GetComponentsInChildren<TrailRenderer>();
    }

    private void Update()
    {
        age += Time.deltaTime;
        OnUpdate();
    }

    protected virtual void OnEnable()
    {
        if (trail != null)
            foreach (var t in trail)
                t.Clear();
    }

    protected virtual void OnDisable()
    {
        age = 0f;
        DamagePacket = new DamagePacket();
        target = null;
    }

    public void Release()
    {
        pool?.Release(this);
    }

    protected abstract void OnUpdate();
}