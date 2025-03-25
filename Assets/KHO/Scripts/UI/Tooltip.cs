using System;
using System.Text.RegularExpressions;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;
    
    [SerializeField] private float tweenDuration = 0.07f;

    private RectTransform _rectTransform;
    
    public LayoutElement layoutElement;
    public int characterWrapLimit;

    private Tween _tween;
    private Image _image;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _image = GetComponent<Image>();
    }

    public void SetText(string content, string header = "")
    {
        if (string.IsNullOrEmpty(header))
        {
            headerField.gameObject.SetActive(false);
        }
        else
        {
            headerField.gameObject.SetActive(true);
            headerField.text = header;
        }
        
        contentField.text = content;
        layoutElement.enabled = CheckStringWrap(headerField.text) || CheckStringWrap(contentField.text);
    }

    private bool CheckStringWrap(string content)
    {
        // Remove all tags except <br>
        string cleanContent = Regex.Replace(content, @"<(?!br>)[^>]*>", "");
        
        // Split the string by '<br>'
        string[] parts = cleanContent.Split(new string[] { "<br>" }, StringSplitOptions.None);

        foreach (string part in parts)
        {
            string trimmedPart = part.Trim();
            if (trimmedPart.Length > characterWrapLimit)
            {
                return true;
            }
        }

        return false;
    }

    private void Update()
    {
        var position = Input.mousePosition;
        
        float pivotX = position.x / Screen.width;
        float pivotY = position.y / Screen.height;
        
        _rectTransform.pivot = new Vector2(pivotX, pivotY);
        transform.position = position;
    }
    
    public void Show()
    {
        _tween?.Kill();
        
        _image?.DOKill();
        headerField?.DOKill();
        contentField?.DOKill();
        
        //_image.color = new Color(1, 1, 1, 0f);
        _image?.DOFade(1f, tweenDuration).SetLink(gameObject);
        headerField?.DOFade(1f, tweenDuration).SetLink(gameObject);
        contentField?.DOFade(1f, tweenDuration).SetLink(gameObject);
    }
    
    public void Hide()
    { 
        _image?.DOKill();
        headerField?.DOKill();
        contentField?.DOKill();
        
        //_image.color = new Color(1, 1, 1, 1f);
        _image?.DOFade(0f, tweenDuration).SetLink(gameObject);
        headerField?.DOFade(0f, tweenDuration).SetLink(gameObject);
        contentField?.DOFade(0f, tweenDuration).SetLink(gameObject);

        _tween?.Kill();
        _tween = DOVirtual.DelayedCall(tweenDuration, () => gameObject.SetActive(false)).SetLink(gameObject);
    }
}
