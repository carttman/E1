using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] public float Damage = float.MinValue;
    public ProjectilePool pool;
    public Tower instigator;
    public Transform Target;
    
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
        Damage = float.MinValue;
        Target = null;
        instigator = null;
    }

    protected abstract void OnUpdate();
}
