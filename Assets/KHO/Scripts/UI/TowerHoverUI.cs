using UnityEngine;
using UnityEngine.UI;

public class TowerHoverUI : MonoBehaviour
{
    [SerializeField] private Image elementImage;
    [SerializeField] private Image star1Image;
    [SerializeField] private Image star2Image;
    [SerializeField] private Image star3Image;
    
    public void UpdateUI(Global.Element element, int star)
    {
        elementImage.sprite = Game.Instance.GlobalData.GetElementIcon(element);
        star1Image.gameObject.SetActive(star >= 1);
        star2Image.gameObject.SetActive(star >= 2);
        star3Image.gameObject.SetActive(star >= 3);
    }
}
