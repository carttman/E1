using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyState : MonoBehaviour
{
    private Animator animator;
    private bool isSlow = false;
    
    public float MoveSpeed = 10f;
    
    void Start()
    { 
        Enemy enemy = gameObject.GetComponent<Enemy>();
        enemy.OnEnemyDied += (enemyInstance, goldDropAmount) => DeadTrigger();
        
        animator = gameObject.GetComponentInChildren<Animator>();
    }

    //사망 이벤트 받고 애니메이션 실행 
    public void DeadTrigger()
    {
        animator.SetTrigger("Dead");
        //체력 UI 비활성화
        Destroy(gameObject, 1f);
    }
    
    public void ApplySlow(float percent, float duration)
    {
        StartCoroutine(Slow(percent, duration));
    }
    
    // 슬로우 디버프 코루틴
    IEnumerator Slow(float percent, float duration)
    {
        if (isSlow)
        {
            yield break;
        }
        isSlow = true;
        MoveSpeed *= percent;
        yield return new WaitForSeconds(duration);
        
        MoveSpeed /= percent;
        isSlow = false;
    }
}
