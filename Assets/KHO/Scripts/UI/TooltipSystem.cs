using System;
using DG.Tweening;
using UnityEngine;

public class TooltipSystem : MonoBehaviour
{
    private static TooltipSystem current;

    public Tooltip tooltip;
    private Tween _tween;

    public void Awake()
    {
        current = this;
    }

    public static void Show(string content, string header = "")
    {
        current.tooltip.SetText(content, header);
        current.tooltip.gameObject.SetActive(true);
        //_tween = current.tooltip.
    }

    public static void Hide()
    {
        current.tooltip.gameObject.SetActive(false);
    }
}
