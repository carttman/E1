using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;

public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string content;
    public string header;

    private Tween _delayTween;
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        _delayTween = DOVirtual.DelayedCall(Game.Instance.tooltipDelay, () => TooltipSystem.Show(content, header)).SetLink(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _delayTween.Kill();
        TooltipSystem.Hide();
    }
}
