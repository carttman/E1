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
    }
    
    private void Start()
    {
        uiImageGO.SetActive(false);
    }

    // Fade In (이미지를 점점 더 선명하게)
    public void FadeIn(float duration = 1.5f)
    {
        uiImageGO.SetActive(true);
        StartCoroutine(FadeImage(uiImage, uiImage.color.a, 1f, duration));
    }

    // Fade Out (이미지를 점점 더 투명하게)
    public void FadeOut(float duration = 1.5f)
    {
        StartCoroutine(FadeImage(uiImage, uiImage.color.a, 0f, duration));
    }

    // 코루틴을 통해 Image의 Color Alpha 값을 조정
    private IEnumerator FadeImage(Image image, float startAlpha, float endAlpha, float duration)
    {
        float time = 0f;
        Color color = image.color;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;

            // Lerp를 통해 Alpha 값을 점진적으로 변화
            color.a = Mathf.Lerp(startAlpha, endAlpha, t);
            image.color = color;

            yield return null;
        }

        // 최종적으로 Alpha 값을 정확히 설정
        color.a = endAlpha;
        image.color = color;
    }
}
