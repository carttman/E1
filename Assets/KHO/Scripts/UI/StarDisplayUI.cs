using UnityEngine;
using UnityEngine.UI;

public class StarDisplayUI : MonoBehaviour
{
    [SerializeField] private Image star1;
    [SerializeField] private Image star2;
    [SerializeField] private Image star3;

    public void ChangeStar(int starCount)
    {
        star1.gameObject.SetActive(starCount >= 1);
        star2.gameObject.SetActive(starCount >= 2);
        star3.gameObject.SetActive(starCount >= 3);
    }
}