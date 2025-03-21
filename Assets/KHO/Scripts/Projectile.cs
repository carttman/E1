using System;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] public float Damage = float.MinValue;
    [SerializeField] protected float speed = 10f;
    [SerializeField] protected float age = 0f;

    public Tower instigator;
    public Transform Target;

    private void Update()
    {
        age += Time.deltaTime;
        OnUpdate();
    }

    protected abstract void OnUpdate();
}
