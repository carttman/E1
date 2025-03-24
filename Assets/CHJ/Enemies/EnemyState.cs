using System;
using System.Collections;
using UnityEngine;

public class EnemyState : MonoBehaviour
{
    private Animator animator;
    
    public float speed = 10f;
    private bool isSlow = false;
    
    void Start()
    { 
        Enemy enemy = gameObject.GetComponent<Enemy>();
        enemy.OnEnemyDied += (enemyInstance, goldDropAmount) => DeadTrigger();
        
        animator = gameObject.GetComponentInChildren<Animator>();
        
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            StartCoroutine(Slow(3, 1f));
        }
    }

    //사망 이벤트 받고 애니메이션 실행 
    public void DeadTrigger()
    {
        animator.SetTrigger("Dead");
        Destroy(gameObject, 1f);
    }
    
    // 슬로우 디버프 코루틴
    IEnumerator Slow(float percent, float duration)
    {
        if (isSlow)
        {
            Debug.Log("isSlow is returned");
            yield return null;
        }
        isSlow = true;
        Debug.Log($"isSlow : {isSlow}");
        speed /= percent;
        yield return new WaitForSeconds(duration);
        
        speed *= percent;
        isSlow = false;
        Debug.Log($"isSlow : {isSlow}");
    }
}
