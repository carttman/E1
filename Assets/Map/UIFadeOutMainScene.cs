using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFadeOutMainScene : MonoBehaviour
{
    [SerializeField]private GameObject uiImageGO;
    private Image uiImage;

    private void Awake()
    {
        uiImage = uiImageGO.GetComponent<Image>();
    }
    
    void Start()
    {
        uiImageGO.SetActive(true);
        FadeOut();
    }

    // 점점 더 투명하게
    public void FadeOut(float duration = 1.5f)
    {
        StartCoroutine(FadeImage(uiImage, uiImage.color.a, 0f, duration));
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

            yield return null;
        }

        color.a = endAlpha;
        image.color = color;
        uiImageGO.SetActive(false);
    }
}
