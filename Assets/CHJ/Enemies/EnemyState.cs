using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using DG.Tweening;

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
        
        // 게임 오브젝트를 1초 동안 아래로 움직이기
        transform.DOMoveY(transform.position.y - 3f, 1f).SetEase(Ease.InOutQuad);

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
