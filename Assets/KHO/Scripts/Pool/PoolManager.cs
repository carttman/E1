using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
[DefaultExecutionOrder(-5000)]
public class PoolManager : MonoBehaviour
{
    [Header("Projectile")] public List<GameObject> projectilePrefabs;

    [Header("Audio")] [SerializeField] public Pool<AudioSource> Audio;

    private Dictionary<GameObject, ProjectilePool> _projectilePools = new();
    public static PoolManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        foreach (var prefab in projectilePrefabs)
        {
            var pool = ProjectilePool.Create(gameObject, prefab);
            _projectilePools.Add(prefab, pool);
        }
    }

    private ProjectilePool GetProjectilePool(GameObject projectilePrefab)
    {
        if (_projectilePools.TryGetValue(projectilePrefab, out var pool)) return pool;

        Debug.LogError($"ProjectilePool for {projectilePrefab.name} not found.");
        return null;
    }

    public Projectile GetProjectile(GameObject projectilePrefab, Vector3 position, Quaternion rotation,
        Transform target, DamagePacket damagePacket)
    {
        var pool = GetProjectilePool(projectilePrefab);
        if (pool != null)
        {
            var proj = pool.Get();
            proj.pool = pool;
            proj.transform.position = position;
            proj.transform.rotation = rotation;
            proj.target = target;
            proj.DamagePacket = damagePacket;
            proj.enabled = true;
            return proj;
        }

        Debug.LogError($"Failed to get projectile from pool for {projectilePrefab.name}.");
        return null;
    }
}