using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Element_Information : MonoBehaviour
{
    private WaveSpawner waveSpawner;
   
    [SerializeField]private Image CurrentElementImage;
    [SerializeField]private Image NextElementImage;
    [SerializeField]private TMP_Text CurrEnemyCount;
    [SerializeField]private TMP_Text NextEnemyCount;

    private Global.Element currWaveEnemyElement;
    private Global.Element nextWaveEnemyElement;
    private void Awake()
    {
        waveSpawner = FindAnyObjectByType<WaveSpawner>();
    }
    
    private void Start()
    {
        waveSpawner.OnClickWaveStartEvent += ElementInfoChanged;
        
        ElementInfoChanged();
    }

    //속성 정보 갱신
    private void ElementInfoChanged()
    {
        // 마지막 웨이브 처리
        if (waveSpawner.WaveIndex == waveSpawner.Waves.Length - 1)
        {
            currWaveEnemyElement = nextWaveEnemyElement;
            CurrentElementImage.sprite = Game.Instance.GlobalData.GetElementIcon(currWaveEnemyElement);
            NextElementImage.enabled = false;
            
            CurrEnemyCount.text = NextEnemyCount.text;
            NextEnemyCount.enabled = false;
            return;
        }

        // 웨이브 스포너 웨이브 정보 => 현재, 다음 웨이브 => 해당 몬스터 속성
        {
            currWaveEnemyElement = waveSpawner.Waves[waveSpawner.WaveIndex].enemy.GetComponent<StatsComponent>().element;
            nextWaveEnemyElement = waveSpawner.Waves[waveSpawner.WaveIndex + 1].enemy.GetComponent<StatsComponent>().element;
            
            CurrentElementImage.sprite = Game.Instance.GlobalData.GetElementIcon(currWaveEnemyElement);
            NextElementImage.sprite = Game.Instance.GlobalData.GetElementIcon(nextWaveEnemyElement);
        }
        
        CurrEnemyCount.text = waveSpawner.Waves[waveSpawner.WaveIndex].SpawnCount.ToString();
        NextEnemyCount.text = waveSpawner.Waves[waveSpawner.WaveIndex + 1].SpawnCount.ToString();
            
    }
}
