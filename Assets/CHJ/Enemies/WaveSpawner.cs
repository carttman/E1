using System;
using UnityEngine;
using System.Collections;
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
 
   public void OnClickWaveStart() //웨이브 시작 버튼 이벤트
   {
       Debug.Log("OnClickWaveStart");

       if (waveIndex != waves.Length) // 모든 웨이브 클리어
       { 
           Btn_WaveStart.interactable = false;
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
       
       Btn_WaveStart.interactable = true;

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
