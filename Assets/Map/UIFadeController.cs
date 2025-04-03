using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class UIFadeController : MonoBehaviour
{
    [SerializeField]private GameObject uiImageGO;
    private Image uiImage;

    private void Awake()
    {
        uiImage = uiImageGO.GetComponent<Image>();
        Time.timeScale = 1;
    }
    
    private void Start()
    {
        uiImageGO.SetActive(false);
    }
    
    public void FadeIn(float duration = 1.5f)
    {
        uiImageGO.SetActive(true);
        StartCoroutine(FadeImage(uiImage, uiImage.color.a, 1f, duration));
    }

   
    private IEnumerator FadeImage(Image image, float startAlpha, float endAlpha, float duration)
    {
        float time = 0f;
        Color color = image.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            image.color = color;
            Debug.Log(time);
            yield return null;
        }

        color.a = endAlpha;
        image.color = color;
    }
}
