using System;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using TMPro;
using UnityEngine;

// 골드 표시 UI
public class GoldUI : MonoBehaviour
{
    private TextMeshProUGUI _textMesh;
    [SerializeField] private Game game;
    [SerializeField] private float tweenTime = 0.5f;

    private TweenerCore<int, int, NoOptions> tween;
    
    private int _displayGold = 0;
    private int _toGold = 0;
    
    private void Awake()
    {
        _textMesh = GetComponent<TextMeshProUGUI>();
        game.goldChanged += GameOngoldChanged;
        _displayGold = game.Gold;
        _toGold = _displayGold;
    }

    private void GameOngoldChanged(int newGold)
    {
        tween?.Kill();

        _toGold = newGold;
        tween = DOTween.To(() => _displayGold,
            x =>
        {
            _displayGold = x;
            _textMesh.text = _displayGold.ToString();
        },
            _toGold,
            tweenTime).SetUpdate(true).SetLink(gameObject);
    }
}
