using System;
using DG.Tweening;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem _current;

    public Tooltip tooltip;
    private Tween _tween;

    public void Awake()
    {
        _current = this;
    }

    public static void Show(string content, string header = "")
    {
        _current.tooltip.SetText(content, header);
        _current.tooltip.gameObject.SetActive(true);
        //_tween = current.tooltip.
    }

    public static void Hide()
    {
        _current.tooltip.gameObject.SetActive(false);
    }
}
