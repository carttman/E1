using System;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] public float Damage = float.MinValue;
    [SerializeField] private float speed = 10f;
    [SerializeField] private float upwardMovementModifier = 0.5f;

    private float maxAllowedDisplacement = 3f;
    
    private float age = 0f;
    private float duration;
    
    private Vector3 startPos;
    private Vector3 middlePos;
    private Vector3 lastKnownTargetPos;
    
    public Transform Target;

    private void Start()
    {
        startPos = transform.position;
        duration = (Target.position - startPos).magnitude / speed;
    }

    private void FixedUpdate()
    {
        age += Time.fixedDeltaTime;
        
        lastKnownTargetPos = Target ? Target.position : lastKnownTargetPos;
        
        var middlePos = Vector3.Lerp(startPos, lastKnownTargetPos, 0.5f) + (lastKnownTargetPos - startPos).magnitude * upwardMovementModifier * Vector3.up;

        var nowPostion = transform.position;
        var wantPosition = FunctionLibrary.Vezier(startPos, middlePos, lastKnownTargetPos, age / duration);
        var displacement = Vector3.Distance(nowPostion, wantPosition);

        if (displacement > maxAllowedDisplacement)
        {
            Destroy(gameObject);
        }
        
        transform.position = wantPosition;
        
        transform.LookAt(duration <= 0.5 ? middlePos : lastKnownTargetPos);
        
        if (age >= duration)
        {
            // Check if target is still present
            if (Target && Target.GetComponent<StatsComponent>() is StatsComponent sc)
            {
                if (!Mathf.Approximately(Damage, float.MinValue))
                {
                    sc.TakeDamage(Damage);
                }
            }
            
            Destroy(gameObject);
        }
    }
}
