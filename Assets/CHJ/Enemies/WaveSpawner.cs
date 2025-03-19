using System;
using UnityEngine;
using System.Collections;
using TMPro;

public class WaveSpawner : MonoBehaviour
{ 
    public event Action<GameObject> OnEnemySpawned;
   public static int EnemiesAlive = 0;
   
   public Wave[] waves; // 웨이브 클래스
   
   //public Transform enemyPrefab;

   public Transform spawnPoint;
   public TextMeshProUGUI waveCountdownText;
   
   //public float timeBetweenWaves = 5f;
   //private float countdown = 2f;

   private int waveIndex = 0;
   
   
   void Update()
   {
       // if (EnemiesAlive > 0) // 몬스터가 살아있다면 리턴
       // {
       //     return;
       // }

       // wave start 버튼 연동
       // 버튼 눌렀을때 웨이브 시작
       
       
       // if (countdown <= 0f) //Spawn Wave 실행
       // {
       //     StartCoroutine(SpawnWave());
       //     countdown = timeBetweenWaves; //카운트 다운을 중간 시간으로 초기화
       //     return;
       // }
       //
       // countdown -= Time.deltaTime;
       //  
       // countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity); //카운트 다운이 0보다 낮아지지 않도록 설정
       //
       // waveCountdownText.text = string.Format("{0:00.00}", countdown); //출력 형식을 지정

       //waveCountdownText.text = Mathf.Round(countdown).ToString();
   }

   public void OnClickWaveStart() //웨이브 시작 버튼 이벤트
   {
       Debug.Log("OnClickWaveStart");

       if (waveIndex != waves.Length) // 모든 웨이브 클리어
       { 
           waveCountdownText.text = string.Format("{0}", waveIndex + 1); //출력 형식을 지정
           StartCoroutine(SpawnWave());
       }

   }

   IEnumerator SpawnWave()
   {
       Wave wave = waves[waveIndex];
       
       waveIndex++; //웨이브 카운트 증가
       for (int i = 0; i < wave.count; i++)  //웨이브 레벨만큼 몬스터 소한
       {
           SpawnEnemy(wave.enemy);
           yield return new WaitForSeconds(1f / wave.rate); // 텀 대기
       }
       
       if (waveIndex == waves.Length) // 모든 웨이브 클리어
       {
           Debug.Log("All waves finished");
           this.enabled = false; // 컴포넌트 비활성화
       }
   }

   void SpawnEnemy(GameObject enemy)
   {
       var newEnemy = Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
       OnEnemySpawned?.Invoke(newEnemy);
       
       EnemiesAlive++; //몬스터 카운트 증가
   }
}
