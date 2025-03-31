using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using Button = UnityEngine.UI.Button;

public class WaveSpawner : MonoBehaviour
{ 
    public event Action<GameObject> OnEnemySpawned;
    public event Action<Enemy> OnThisWaveFinished; 
   
   [SerializeField] private Wave[] waves; // 웨이브 클래스
   [SerializeField] private Transform spawnPoint;
   [SerializeField] private TextMeshProUGUI waveCountdownText;
   [SerializeField] private Button Btn_WaveStart;
   [SerializeField] private Slider waveSlider;

    private int WaveIndex = 0; 
    private bool isSpawnFinished = false;
    
    public static int CurrentEnemiesAlive = 0;
    
    public static List<int> WaveList = new List<int>();
   
   private void Awake()
   {
        SetWaveList();
   }
   private void Update()
   {
        CheckWaveFinished();
   }

   //게임 시작 시, 웨이브, 몬스터 수 저장
   private void SetWaveList()
   {
       for (int i = 0; i < waves.Length; i++)
       {
           WaveList.Add(waves[i].SpawnCount);
       }
   }

   public void OnClickWaveStart() //웨이브 시작 버튼 이벤트
   {
       if (WaveIndex != waves.Length) // 모든 웨이브 클리어
       { 
           Btn_WaveStart.interactable = false;
           waveCountdownText.text = $"{WaveIndex + 1} / {waves.Length}";
           StartCoroutine(SpawnWave());
           waveSlider.value = 0f;
       }
   }

    IEnumerator SpawnWave()
   {
       Wave wave = waves[WaveIndex];
       
       WaveIndex++; //웨이브 카운트 증가
       for (int i = 0; i < wave.SpawnCount; i++)  //웨이브 레벨만큼 몬스터 소한
       {
           isSpawnFinished = false;
           WaveToSpawnEnemy(wave.enemy);
           yield return new WaitForSeconds(1f / wave.SpawnRate); // 텀 대기
       }
       
       isSpawnFinished = true;
       Btn_WaveStart.interactable = true;
   }

   private void WaveToSpawnEnemy(GameObject enemy)
   {
       var newEnemyGO = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
       OnEnemySpawned?.Invoke(newEnemyGO);
       
       var newEnemy = newEnemyGO.GetComponent<Enemy>();
       newEnemy.MyWaveIndex = WaveIndex;
       
       // 사망 이벤트
       newEnemy.OnEnemyDied += (enemyInstance, _) => OnNewEnemyDied(enemyInstance);
       newEnemy.OnEnemyEndPath += (enemyInstance, _) => OnNewEnemyDied(enemyInstance);
       
       CurrentEnemiesAlive++; //몬스터 카운트 증가
   }

   private void OnNewEnemyDied(Enemy enemy)
   {
       WaveList[enemy.MyWaveIndex - 1]--;

       if (WaveIndex == enemy.MyWaveIndex)
       {
           waveSlider.value = 1.0f - (1.0f * WaveList[enemy.MyWaveIndex - 1] / waves[enemy.MyWaveIndex - 1].SpawnCount);
       }
       
       if (WaveList[enemy.MyWaveIndex - 1] <= 0)
       {
           OnThisWaveFinished?.Invoke(enemy);
       }
   }
   
   // 설정된 모든 웨이브 클리어 했는지 판단
   private void CheckWaveFinished()
   {
       if (WaveIndex == waves.Length)
       {
           if (CurrentEnemiesAlive <= 0)
           {  
               if (isSpawnFinished)
               { 
                   //Clear UI 활성화
                   Game.Instance.ClearGame();
                   this.enabled = false; // 컴포넌트 비활성화
               }
           }
       }
   }
   
}
