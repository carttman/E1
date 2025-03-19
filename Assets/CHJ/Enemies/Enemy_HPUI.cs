using UnityEngine;
using UnityEngine.UI;

public class Enemy_HPUI : MonoBehaviour
{
    [Header("Unity Stuff")]
    public Image healthBar;
    public Camera MyCamera;
    public Canvas HPCanvas;

    StatsComponent statsComponent;
    private void Awake()
    {
        MyCamera = Camera.main;
        HPCanvas.worldCamera = MyCamera;
        statsComponent= GetComponent<StatsComponent>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   //HealthChanged 이벤트 바인딩
        statsComponent.HealthChanged += hp => HP_Update(hp, statsComponent.MaxHealth);
    }
    
    // Update is called once per frame
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
}
