using System.Collections.Generic;
using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    protected SphereCollider SphereCollider;
    [SerializeField] public TowerData towerData;
    
    [SerializeField, Range(1.5f, 100f)]
    protected float targetingRange = 1.5f;
    
    [SerializeField] protected List<Transform> potentialTargets = new List<Transform>();
    [SerializeField] protected float damagePerSecond = 10f;
    
    [SerializeField] protected Transform rotatingPart;
    [SerializeField] protected Transform turret;

    
    protected void Awake()
    {
        Debug.Assert(towerData);
        
        SphereCollider = GetComponent<SphereCollider>();
        SphereCollider.radius = targetingRange;
    }
    
    protected bool AcquireTarget(out Transform pTarget)
    {
        if (potentialTargets.Count == 0)
        {
            pTarget = null;
            return false;
        }

        for (int i = 0; i < potentialTargets.Count; i++)
        {
            if (potentialTargets[i])
            {
                pTarget = potentialTargets[i];
                return true;
            }
            else
            {
                potentialTargets.RemoveAt(i);
                i--;
            }
        }
        pTarget = null;
        return false;
    }

    protected bool TrackTarget(ref Transform pTarget)
    {
        if (!pTarget) return false;
        
        if ((transform.position - pTarget.position).magnitude <= targetingRange)
        {
            // look at target
            turret.LookAt(pTarget.position);
            return true;
        }
        return false;
    }
    
    protected void OnTriggerEnter(Collider other)
    {
        //Debug.Log($"{other.name} has entered the tower range");
        if (other.CompareTag("Enemy"))
        {
            potentialTargets.Add(other.transform);
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (potentialTargets.Contains(other.transform))
        {
            potentialTargets.Remove(other.transform);
        }
    }

    protected void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 position = transform.localPosition;
        position.y += 0.01f;
        Gizmos.DrawWireSphere(position, targetingRange);
    }
}
