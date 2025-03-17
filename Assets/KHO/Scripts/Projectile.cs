using System;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float moveSpeedPerSecond = 1f;

    public Transform target;
    public Vector3 startPosition;
    public float damage;
    
    private float progress = 0f;
    private float moveDuration;
    
    private void Start()
    {
        moveDuration = (target.transform.position - transform.position).magnitude / moveSpeedPerSecond;
        startPosition = transform.position;
    }

    private void Update()
    {
        if (!target)
        {
            Destroy(gameObject);
        }
        
        progress += Time.deltaTime;
        transform.position = Vector3.Lerp(startPosition, target.transform.position, (progress / moveDuration));

        if (progress >= moveDuration)
        {
            StatsComponent targetStatsComponent = target?.GetComponent<StatsComponent>();

            if (targetStatsComponent)
            {
                targetStatsComponent.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}
