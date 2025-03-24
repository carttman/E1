using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour
{
    public TextMeshProUGUI headerField;
    public TextMeshProUGUI contentField;

    private RectTransform _rectTransform;
    
    public LayoutElement layoutElement;
    public int characterWrapLimit;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
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
}
