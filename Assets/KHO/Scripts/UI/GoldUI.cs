using DG.Tweening;
using TMPro;
using UnityEngine;

// 골드 표시 UI
public class GoldUI : MonoBehaviour
{
    [SerializeField] private float tweenTime = 0.5f;

    private int _displayGold;
    private Tween _scaleTween;

    private TextMeshProUGUI _textMesh;
    private int _toGold;

    private Tween _valueTween;

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