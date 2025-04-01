using UnityEngine;
using UnityEngine.UI;

public class Element_Information : MonoBehaviour
{
    private WaveSpawner waveSpawner;
   
    [SerializeField]private Image CurrentElementImage;
    [SerializeField]private Image NextElementImage;

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
        // 다음 웨이브까지 표시됐으면 업데이트 중지
        if (waveSpawner.WaveIndex == waveSpawner.Waves.Length - 1) return;
        
        // 웨이브 스포너 웨이브 정보 => 현재, 다음 웨이브 => 해당 몬스터 속성
        currWaveEnemyElement = waveSpawner.Waves[waveSpawner.WaveIndex].enemy.GetComponent<StatsComponent>().element;
        nextWaveEnemyElement = waveSpawner.Waves[waveSpawner.WaveIndex + 1].enemy.GetComponent<StatsComponent>().element;
        
        // if (waveSpawner.WaveIndex + 1 > waveSpawner.Waves.Length)
        // {
        //     currWaveEnemyElement = nextWaveEnemyElement;
        //     NextElementImage.enabled = false;
        //     
        // }
        
        CurrentElementImage.sprite = Game.Instance.GlobalData.GetElementIcon(currWaveEnemyElement);
        NextElementImage.sprite = Game.Instance.GlobalData.GetElementIcon(nextWaveEnemyElement);
            
    }
}
