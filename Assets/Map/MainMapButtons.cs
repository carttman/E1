using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMapButtons : MonoBehaviour
{
    public void OnClickStart()
    {
        SceneManager.LoadScene("MainScene");
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
