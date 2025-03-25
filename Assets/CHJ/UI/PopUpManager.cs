using TMPro;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    private static PopUpManager _instance;
    public static PopUpManager Instance
    {
        get { return _instance; }
    }
    
    public GameObject PopUpObject;
    private TMP_Text PopUpText;

    private void Awake()
    {
        _instance = this;
    }
    
    public void CreatePopUpUI(string amount, Vector3 objectTcVector3, Color color, float size)
    {
        Vector3 randomPos = new Vector3(Random.Range(0f, .5f), Random.Range(0f, .5f), Random.Range(0f, .5f));
        
        GameObject popUp = Instantiate(PopUpObject, objectTcVector3 + randomPos, Quaternion.identity);
        
        TextMeshProUGUI temp = popUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        temp.text = amount;
        temp.color = color;
        temp.fontSize = size;

        Destroy(popUp, 1f);
    }
}
