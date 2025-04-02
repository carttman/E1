using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMapButtons : MonoBehaviour
{
    public void OnClickStart()
    {
        AudioManager.instance.PlaySound(SoundEffect.ButtonClick);
        SceneManager.LoadScene("MainScene");
    }

    public void OnClickExit()
    {
        AudioManager.instance.PlaySound(SoundEffect.ButtonClick);
        if (Application.isEditor)
        {
            Debug.Log("Application would quit here in a build");
        }
        Application.Quit();
    }
}
