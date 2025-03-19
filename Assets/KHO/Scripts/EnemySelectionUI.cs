using TMPro;
using UnityEngine;

public class EnemySelectionUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _enemyNameText;
    [SerializeField] private TextMeshProUGUI _healthText;
    [SerializeField] private TextMeshProUGUI _moveSpeedText;

    private void Awake()
    {
        Debug.Assert(_enemyNameText);
        Debug.Assert(_healthText);
        Debug.Assert(_moveSpeedText);
    }

    public void HandleUIChange(EnemySelectionData data)
    {
        _enemyNameText.text = data.Name;
        _healthText.text = $"HP: {data.Health} / {data.MaxHealth}";
        _moveSpeedText.text = $"Move Speed: {data.MoveSpeed}";
    }
}
