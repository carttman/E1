using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class BuildableTileEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        GameEventHub.Instance.TilePointerEnter(transform);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameEventHub.Instance.TilePointerExit(transform);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        GameEventHub.Instance.TilePointerClick(transform);
    }
}
