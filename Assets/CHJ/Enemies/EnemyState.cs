using UnityEngine;

public class EnemyState : MonoBehaviour
{
    private Animator animator;
    
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
        Destroy(gameObject, 1f);
    }
}
