using UnityEngine;

public class BezierProjectile : Projectile
{
    [SerializeField] private float upwardMovementModifier = 0.5f;

    private float maxAllowedDisplacement = 3f;
    
    private float duration;
    
    private Vector3 startPos;
    private Vector3 middlePos;
    private Vector3 lastKnownTargetPos;

    private Vector3 targetPos;
    
    private void Start()
    {
        startPos = transform.position;
        middlePos = Vector3.Lerp(startPos, Target.position, 0.5f) + (Target.position - startPos).magnitude * upwardMovementModifier * Vector3.up;
        duration = (Target.position - startPos).magnitude / speed;
    }

    private void FixedUpdate()
    {
        lastKnownTargetPos = Target ? Target.position : lastKnownTargetPos;
        
        var nowPosition = transform.position;
        var wantPosition = FunctionLibrary.Bezier(startPos, middlePos, lastKnownTargetPos, age / duration);
        var displacement = Vector3.Distance(nowPosition, wantPosition);

        if (displacement > maxAllowedDisplacement)
        {
            Destroy(gameObject);
        }
        
        transform.position = wantPosition;
        //transform.LookAt(duration <= 0.5 ? middlePos : lastKnownTargetPos);
        transform.LookAt(Target);
        
        if (age >= duration)
        {
            // Check if target is still present
            if (Target && Target.GetComponent<StatsComponent>() is StatsComponent sc)
            {
                if (!Mathf.Approximately(Damage, float.MinValue))
                {
                    sc.TakeDamage(Damage, instigator);
                }
            }
            
            Destroy(gameObject);
        }
    }

    protected override void OnUpdate() { }
}
