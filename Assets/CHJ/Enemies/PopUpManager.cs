using System.Collections;
using TMPro;
using UnityEngine;

public class PopUpManager : MonoBehaviour
{
    private static PopUpManager _instance;
    public static PopUpManager Instance
    {
        get { return _instance; }
    }
    
    private TMP_Text PopUpText;
    public GameObject PopUpObject;
    private Camera MyCamera;

    //private GameObject PopUpObject;
    private void Awake()
    {
        _instance = this;
        MyCamera = Camera.main;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PopUpText = PopUpObject.GetComponentInChildren<TMP_Text>();
        Debug.Log(PopUpText.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PopUpDamageUI(float damage, Transform objectTransform)
    {
        StartCoroutine(CreatePopUp(damage, objectTransform));
    }

    IEnumerator CreatePopUp(float damage, Transform objectTransform)
    {
        Vector3 pos = MyCamera.transform.position - objectTransform.position;
        Vector3 newVec = new Vector3(pos.x, pos.y, 0);
        
        PopUpText.text = $"{damage}";
        var widget = Instantiate(PopUpObject, objectTransform.position, Quaternion.LookRotation(-newVec.normalized));
        yield return new WaitForSeconds(1);
        Destroy(widget);
    }
}
