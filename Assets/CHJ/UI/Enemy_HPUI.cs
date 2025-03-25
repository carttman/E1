using UnityEngine;
using UnityEngine.UI;

public class Enemy_HPUI : MonoBehaviour
{
    [Header("Enemy_HPUI")]
    public Image healthBar;
    public Camera MyCamera;
    public Canvas HPCanvas;

    private void Awake()
    {
        MyCamera = Camera.main;
        HPCanvas.worldCamera = MyCamera;
        
        StatsComponent statsComponent= GetComponent<StatsComponent>();
        statsComponent.HealthChanged += hp => HP_Update(hp, statsComponent.MaxHealth);
        
        Enemy enemy = gameObject.GetComponent<Enemy>();
        enemy.OnEnemyDied += (enemyInstance, goldDropAmount) => DeadTriggerToHpBar();
    }
    
    void Update()
    {
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
