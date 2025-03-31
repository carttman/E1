using UnityEngine;
using UnityEngine.UI;

public class Enemy_HPUI : MonoBehaviour
{
    [Header("Enemy_HPUI")]
    [SerializeField] private Image healthBar;
    [SerializeField] private Canvas HPCanvas;
    private Camera MyCamera;

    private void Awake()
    {
        MyCamera = Camera.main;
        HPCanvas.worldCamera = MyCamera;
        
        StatsComponent statsComponent= GetComponent<StatsComponent>();
        statsComponent.HealthChanged += hp => HP_Update(hp, statsComponent.maxHealth);
        
        Enemy enemy = gameObject.GetComponent<Enemy>();
        enemy.OnEnemyDied += (enemyInstance, goldDropAmount) => DeadTriggerToHpBar();
    }
    
    void Update()
    {
        //HPBar의 방향 업데이트
        HPCanvas.transform.forward = -MyCamera.transform.forward;
    }

    void HP_Update(float health, float maxHealth)
    {
        healthBar.fillAmount = health / maxHealth;
    }

    //사망 이벤트 발생 시 HP바 비활성화
    void DeadTriggerToHpBar()
    {
        HPCanvas.enabled = false;
    }
}
