using System;
using UnityEngine;
using System.Collections;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
   public static int EnemiesAlive = 0;
   
   public Wave[] waves;
   
   public Transform enemyPrefab;

   public Transform spawnPoint;
   
   public TextMeshProUGUI waveCountdownText;
   
   public float timeBetweenWaves = 5f;
   private float countdown = 2f;

   private int waveIndex = 0;
   
   void Update()
   {
       if (EnemiesAlive >0)
       {
           return;
       }

       if (countdown <= 0f) //카운트다운이 0 보다 작아지먄 Spawn Wave 실행
       {
        
           StartCoroutine(SpawnWave());
           countdown = timeBetweenWaves; //카운트 다운을 중간 시간으로 초기화
           return;
       }

       //deltaTime//마지막 프레임을 그린 후 경과한 시간
       countdown -= Time.deltaTime; //시간을 계속 줄인다.
        
       countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity); //카운트 다운이 0보다 낮아지지 않도록 설정

       waveCountdownText.text = string.Format("{0:00.00}", countdown); //출력 형식을 지정

       //waveCountdownText.text = Mathf.Round(countdown).ToString();
   }


   IEnumerator SpawnWave()
   {
       Wave wave = waves[waveIndex];
       
       for (int i = 0; i < wave.count; i++)  //웨이브 레벨만큼 몬스터 소한
       {
           SpawnEnemy(wave.enemy);
           yield return new WaitForSeconds(1f / wave.rate);
       }

       waveIndex++;

       if (waveIndex == waves.Length)
       {
           Debug.Log("Wave Win");
           this.enabled = false;
       }
   }

   void SpawnEnemy(GameObject enemy)
   {
       Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
       EnemiesAlive++; //몬스터 수 증가

   }
   
}
