using UnityEngine;
using UnityEngine.Pool;

public class ProjectilePool: MonoBehaviour
{
    /// <summary>
    /// The prefab to be used for the pool. This prefab must have a component of type T attached to it.
    /// </summary>
    public GameObject prefab;
    /// <summary>
    /// The default capacity the stack will be created with.
    /// </summary>
    [SerializeField] private int defaultCapacity = 4;
    /// <summary>
    ///The maximum size of the pool. When the pool reaches the max size then any further instances returned to the pool will be ignored and can be garbage collected. This can be used to prevent the pool growing to a very large size.
    /// </summary>
    [SerializeField] private int maxSizeInactive = 128;
    
    private IObjectPool<Projectile> _pool;

    protected void Awake()
    {
        _pool = new ObjectPool<Projectile>(CreateObject, OnGet, OnRelease, DestroyObject,
            true, defaultCapacity, maxSizeInactive);
    }
    
    public static ProjectilePool Create(GameObject gameObject, GameObject prefab)
    {
        ProjectilePool newPool = gameObject.AddComponent<ProjectilePool>();
        newPool.prefab = prefab;
        return newPool;
    }
    
    public Projectile Get()
    { 
        return _pool.Get();
    }

    public void Release(Projectile obj)
    {
        _pool.Release(obj);
    }
    
    private Projectile CreateObject()
    {
        var go = Instantiate(prefab);
        var projectile = go.GetComponent<Projectile>();
        return projectile;
    }

    private void OnGet(Projectile obj) { }
    private void OnRelease(Projectile obj)
    {
        obj.gameObject.SetActive(false);
    }
    private void DestroyObject(Projectile obj)
    {
        Destroy(obj.gameObject);
    }
}