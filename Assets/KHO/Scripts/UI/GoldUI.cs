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
    [SerializeField] private float tweenTime = 0.5f;

    private Tween _tween;
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
        _tween?.Kill();

        _toGold = newGold;
        _tween = DOTween.To(() => _displayGold,
            x =>
        {
            _displayGold = x;
            _textMesh.text = _displayGold.ToString();
        },
            _toGold,
            tweenTime).SetUpdate(true).SetLink(gameObject);
    }
}
