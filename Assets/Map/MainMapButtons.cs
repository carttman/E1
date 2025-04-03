using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMapButtons : MonoBehaviour
{
    [SerializeField]private UIFadeController fadeController;
    public void OnClickStart()
    {
        fadeController.FadeIn();
        StartCoroutine(LoadSceneAfterFade("MainScene"));
    }

    private IEnumerator LoadSceneAfterFade(string sceneName)
    {
        yield return new WaitForSecondsRealtime(1.5f); 
        SceneManager.LoadScene(sceneName);
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
