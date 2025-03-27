using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Button = UnityEngine.UI.Button;

public class WaveSpawner : MonoBehaviour
{ 
    public event Action<GameObject> OnEnemySpawned;
    public static int EnemiesAlive = 0;
   
   public Wave[] waves; // 웨이브 클래스

   public Transform spawnPoint;
   public TextMeshProUGUI waveCountdownText;

   private int waveIndex = 0;
    
   public Button Btn_WaveStart;

   private bool isSpawnFinished = false;
   
   //private List<List<Enemy>> WaveList = new List<List<Enemy>>();
   

   private void Update()
   {
      CheckWaveFinished();
   }

   public void OnClickWaveStart() //웨이브 시작 버튼 이벤트
   {
       if (waveIndex != waves.Length) // 모든 웨이브 클리어
       { 
           Btn_WaveStart.interactable = false;
           waveCountdownText.text = string.Format("{0}", waveIndex + 1); //출력 형식을 지정

           // List<Enemy> enemies = new List<Enemy>();
           // WaveList.Add(enemies);
           
           StartCoroutine(SpawnWave());
       }
   }

    IEnumerator SpawnWave()
   {
       Wave wave = waves[waveIndex];
       
       waveIndex++; //웨이브 카운트 증가
       for (int i = 0; i < wave.SpawnCount; i++)  //웨이브 레벨만큼 몬스터 소한
       {
           isSpawnFinished = false;
           SpawnEnemy(wave.enemy);
           yield return new WaitForSeconds(1f / wave.SpawnRate); // 텀 대기
       }
       
       isSpawnFinished = true;
       Btn_WaveStart.interactable = true;
   }

   void SpawnEnemy(GameObject enemy)
   {
       var newEnemyGO = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
       OnEnemySpawned?.Invoke(newEnemyGO);
       
       // 사망 이벤트 바인딩
       var newEnemy = newEnemyGO.GetComponent<Enemy>();
       //newEnemy.OnEnemyDied += NewEnemyOnOnEnemyDied;
       
       //  enemy 객체 카운팅
       //WaveList[waveIndex-1].Add(newEnemy);
       
       EnemiesAlive++; //몬스터 카운트 증가
   }

   // private void NewEnemyOnOnEnemyDied(Enemy enemy, float golddropamount)
   // {
   //     
   //     foreach (var Wave in WaveList)
   //     {
   //         if (Wave.Contains(enemy))
   //         {
   //              Wave.Remove(enemy);
   //              if (Wave.Count <= 0)
   //              {
   //                  
   //              }
   //              break;
   //         }
   //     }
   // }


   void CheckWaveFinished()
   {
       if (waveIndex == waves.Length)
       {
           if (EnemiesAlive <= 0)
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
   
   // 각 웨이브에 스폰된 몬스터들 체크
   // 해당 웨이브에 있는 몬스터를 전부 처리하면 작동되는 이벤트 필요
   
   // 해당 웨이브의 몬스터가 스폰될때 각 몬스터 객체에 웨이브카운트 저장한다.
   
}
