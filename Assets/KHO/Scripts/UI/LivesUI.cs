using DG.Tweening;
using TMPro;
using UnityEngine;

// 목숨 표시 UI
public class LivesUI : MonoBehaviour
{
    private RectTransform _rect;
    private TextMeshProUGUI _textMesh;

    private void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
        _rect = GetComponent<RectTransform>();
    }

    private void Start()
    {
        Game.Instance.LivesChanged += OnLivesChanged;
    }

    private void OnLivesChanged(int newLives)
    {
        _textMesh.text = $"{newLives}";
        _rect.DOComplete();
        _rect.DOPunchScale(new Vector3(0.25f, 0.25f, 0.25f), 0.25f)
            .SetUpdate(true).SetLink(gameObject);
    }
}