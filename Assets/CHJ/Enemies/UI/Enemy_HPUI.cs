using UnityEngine;
using UnityEngine.UI;

public class Enemy_HPUI : MonoBehaviour
{
    [Header("Enemy_HPUI")]
    public Image healthBar;
    public Camera MyCamera;
    public Canvas HPCanvas;

    StatsComponent statsComponent;
    private void Awake()
    {
        MyCamera = Camera.main;
        HPCanvas.worldCamera = MyCamera;
        statsComponent= GetComponent<StatsComponent>();
        
        Enemy enemy = gameObject.GetComponent<Enemy>();
        enemy.OnEnemyDied += (enemyInstance, goldDropAmount) => DeadTriggerToHpBar();
    }
    
    void Start()
    {   //HealthChanged 이벤트 바인딩
        statsComponent.HealthChanged += hp => HP_Update(hp, statsComponent.MaxHealth);
    }
    
    void Update()
    {
        Vector3 dir = MyCamera.transform.position - HPCanvas.transform.position;
        Vector3 newVec = new Vector3(dir.x, dir.y, 0);
        HPCanvas.transform.rotation = Quaternion.LookRotation(newVec.normalized);
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
