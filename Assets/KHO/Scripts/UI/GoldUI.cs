using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

// 골드 표시 UI
public class GoldUI : MonoBehaviour
{
    [SerializeField] private float tweenTime = 0.5f;

    private TextMeshProUGUI _textMesh;
    
    private Tween _valueTween;
    private Tween _scaleTween;
    
    private int _displayGold = 0;
    private int _toGold = 0;
    
    private void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        Game.Instance.GoldChanged += GameOnGoldChanged;
        _displayGold = Game.Instance.Gold;
        _toGold = _displayGold;
    }

    private void GameOnGoldChanged(int newGold)
    {
        _valueTween?.Kill();
        
        _toGold = newGold;
        _valueTween = DOTween.To(() => _displayGold,
            x =>
        {
            _displayGold = x;
            _textMesh.text = _displayGold.ToString();
        },
            _toGold,
            tweenTime).SetUpdate(true).SetLink(gameObject);

        _scaleTween?.Kill(true);
        _scaleTween = _textMesh.rectTransform.DOPunchScale(new Vector3(0.05f, 0.05f, 0.05f), 0.25f)
                                             .SetUpdate(true).SetLink(gameObject);
        
    }
}
