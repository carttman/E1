using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemySelectionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI enemyNameText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI moveSpeedText;
    [SerializeField] private Image elementImage;

    private void Awake()
    {
        Debug.Assert(enemyNameText);
        Debug.Assert(healthText);
        Debug.Assert(moveSpeedText);
        Debug.Assert(elementImage);
    }

    public void HandleUIChange(EnemySelectionData data)
    {
        enemyNameText.text = data.Name;
        healthText.text = $"HP: {data.Health:0}/{data.MaxHealth:0}";
        moveSpeedText.text = $"이동속도: {data.MoveSpeed}";
        elementImage.sprite = Game.Instance.GlobalData.GetElementIcon(data.Element);
    }
}