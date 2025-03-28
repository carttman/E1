using System;
using UnityEngine;
using UnityEngine.Pool;

[DefaultExecutionOrder(-2000)]
public class Pool<T> : MonoBehaviour
    where T : Component
{
    /// <summary>
    /// The prefab to be used for the pool. This prefab must have a component of type T attached to it.
    /// </summary>
    [SerializeField] private GameObject prefab;
    /// <summary>
    /// The default capacity the stack will be created with.
    /// </summary>
    [SerializeField] private int defaultCapacity = 4;
    /// <summary>
    ///The maximum size of the pool. When the pool reaches the max size then any further instances returned to the pool will be ignored and can be garbage collected. This can be used to prevent the pool growing to a very large size.
    /// </summary>
    [SerializeField] private int maxSizeInactive = 64;
    private IObjectPool<T> _pool;
    
    protected void Awake()
    {
        _pool = new ObjectPool<T>(CreateObject, OnGet, OnRelease, DestroyObject,
            false, defaultCapacity, maxSizeInactive);
    }
    
    public T Get()
    {
        return _pool.Get();
    }

    public void Release(T obj)
    {
        _pool.Release(obj);
    }
    
    protected virtual T CreateObject()
    {
        var go = Instantiate(prefab);
        return go.GetComponent<T>();
    }

    protected virtual void OnGet(T obj)
    {
        obj.gameObject.SetActive(true);
    }
    
    protected virtual void OnRelease(T obj)
    {
        obj.gameObject.SetActive(false);
    }
    
    protected virtual void DestroyObject(T obj)
    {
        Destroy(obj.gameObject);
    }
}
