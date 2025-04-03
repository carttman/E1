using TMPro;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    private static PopUpManager _instance;
    public static PopUpManager Instance
    {
        get { return _instance; }
    }
    
    [SerializeField] private GameObject PopUpObject;
    private TMP_Text PopUpText;

    private void Awake()
    {
        _instance = this;
    }
    
    public void CreatePopUpUI(string amount ,Vector3 objectVector, Color color, float size, string damageScale = " ")
    {
        Vector3 randomPos = new Vector3(Random.Range(0f, .5f), Random.Range(0f, .5f), Random.Range(0f, .5f));
        
        GameObject popUp = Instantiate(PopUpObject, objectVector + randomPos, Quaternion.identity);
        
        TextMeshProUGUI temp = popUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        
        // 소수점 1자리로 변환 (골드 색상 제외)
        if (color != Color.yellow)
        {
            if (float.TryParse(amount, out float parsedAmount))
            {
                amount = parsedAmount.ToString("F1");
            }
        }
        
        temp.text = $"{amount}<size=60%><color=#00ff00ff>{damageScale}</color></size>";
        temp.color = color;
        temp.fontSize = size;
        
        Destroy(popUp, 1f);
    }
}
