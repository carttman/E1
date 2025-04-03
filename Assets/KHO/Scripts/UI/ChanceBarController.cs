using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChanceBarController : MonoBehaviour
{
    [SerializeField] private LayoutElement segment1LayoutElement;
    [SerializeField] private TextMeshProUGUI segment1Text;

    [SerializeField] private LayoutElement segment2LayoutElement;
    [SerializeField] private TextMeshProUGUI segment2Text;

    [SerializeField] private LayoutElement segment3LayoutElement;
    [SerializeField] private TextMeshProUGUI segment3Text;

    [Header("Result Indicator")] [SerializeField]
    private RectTransform resultIndicator;
    [SerializeField] private float animationDuration = 0.75f;
    [SerializeField] private RectTransform container;

    [Header("Appear Tweening")]
    private Tween _appearTween;
    private Tween _disappearTimer;

    [SerializeField] private float appearEndY = 15f;
    [SerializeField] private float appearStartY = -30f;
    [SerializeField] private Ease appearEaseType = Ease.OutCubic;
    [SerializeField] private float appearDuration = 0.5f;
    
    [SerializeField] private Ease disappearEaseType = Ease.InCubic;
    [SerializeField] private float disappearDelay = 3f;
    
    private void Start()
    {
        OnBuildLevelChanged(Game.Instance.BuildLevel);
        Game.Instance.BuildLevelChanged += OnBuildLevelChanged;
        Game.Instance.RarityRolled += OnRarityRolled;

        resultIndicator.gameObject.SetActive(false);
        
        var buildButtons = FindObjectsByType<TowerBuildButtonUI>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var button in buildButtons)
        {
            button.HoveredOnTowerBuildButton += Appear;
            button.ExitedTowerBuildButton += StartDisappearTimer;
        }
    }

    private void Appear()
    {
        // 끝까지 사라졌을때만 실행
        if (_appearTween != null)
        {
            return;
        }
        
        _appearTween = transform.DOLocalMoveY(appearEndY, appearDuration)
            .SetEase(appearEaseType)
            .SetUpdate(true)
            .SetLink(gameObject);
    }

    private void StartDisappearTimer()
    {
        if (_disappearTimer != null && _disappearTimer.IsActive())
        {
            _disappearTimer.Kill();
        }
        _disappearTimer = DOVirtual.DelayedCall(disappearDelay, Disappear)
                            .SetUpdate(true)
                            .SetLink(gameObject);
    }

    private void Disappear()
    {
        transform.DOKill(true);
        
        transform.DOLocalMoveY(appearStartY, appearDuration)
            .SetEase(disappearEaseType)
            .SetUpdate(true)
            .SetLink(gameObject);
        
        _appearTween?.Kill();
        _appearTween = null;
    }

    private void UpdateGauge(float chance1, float chance2, float chance3)
    {
        var totalChance = chance1 + chance2 + chance3;
        if (!Mathf.Approximately(totalChance, 1f))
        {
            Debug.LogWarning("Total chance is not equal to 1");
            return;
        }

        segment1LayoutElement.flexibleWidth = chance1;
        segment2LayoutElement.flexibleWidth = chance2;
        segment3LayoutElement.flexibleWidth = chance3;

        segment1Text.text = $"{chance1 * 100f:F0}%";
        segment2Text.text = $"{chance2 * 100f:F0}%";
        segment3Text.text = $"{chance3 * 100f:F0}%";

        segment1LayoutElement.gameObject.SetActive(chance1 > 0.001f);
        segment2LayoutElement.gameObject.SetActive(chance2 > 0.001f);
        segment3LayoutElement.gameObject.SetActive(chance3 > 0.001f);
    }

    private void OnBuildLevelChanged(int newBuildLevel)
    {
        if (newBuildLevel < 0 || newBuildLevel >= Game.Instance.GlobalData.towerRateChance.Length)
        {
            Debug.LogError($"Invalid build level: {newBuildLevel}");
            return;
        }

        var rateChance = Game.Instance.GlobalData.towerRateChance[newBuildLevel];
        UpdateGauge(rateChance.oneStarRate, rateChance.twoStarRate, rateChance.threeStarRate);
    }

    private void OnRarityRolled(float roll)
    {
        if (roll < 0f || roll > 1f)
        {
            Debug.LogError($"Invalid roll value: {roll}");
            return;
        }
        
        resultIndicator.gameObject.SetActive(true);

        var containerWidth = container.rect.width;
        var targetAnchoredX = roll * containerWidth;

        var currentPosition = resultIndicator.anchoredPosition;
        currentPosition.x = 0f;
        resultIndicator.anchoredPosition = currentPosition;
        resultIndicator.gameObject.SetActive(true);

        resultIndicator.DOKill();

        resultIndicator.DOAnchorPosX(targetAnchoredX, animationDuration)
            .SetEase(Ease.OutCirc)
            .SetUpdate(true)
            .SetLink(gameObject);
    }
}