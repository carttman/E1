using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMap : MonoBehaviour
{
    public Button Btn_Start;
    public Button Btn_Exit;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void OnClickStart()
    {
        SceneManager.LoadScene("CHJ_NewNewTile");
    }

    public void OnClickExit()
    {
        if (Application.isEditor)
        {
            Debug.Log("Application would quit here in a build");
        }
        Application.Quit();
    }
}
