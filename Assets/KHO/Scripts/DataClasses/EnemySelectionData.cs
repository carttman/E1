using UnityEngine;

public class EnemySelectionData : SelectionData
{
    private float _health;

    private float _maxHealth;

    private float _moveSpeed;

    private string _name;
    public Global.Element Element;

    public string Name
    {
        get => _name;
        set
        {
            if (_name == value) return;
            _name = value;
            OnSelectionDataChanged?.Invoke(this);
        }
    }

    public float MaxHealth
    {
        get => _maxHealth;
        set
        {
            if (Mathf.Approximately(value, _maxHealth)) return;
            _maxHealth = value;
            OnSelectionDataChanged?.Invoke(this);
        }
    }

    public float Health
    {
        get => _health;
        set
        {
            if (Mathf.Approximately(value, _health)) return;
            _health = value;
            OnSelectionDataChanged?.Invoke(this);
        }
    }

    public float MoveSpeed
    {
        get => _moveSpeed;
        set
        {
            if (Mathf.Approximately(value, _moveSpeed)) return;
            _moveSpeed = value;
            OnSelectionDataChanged?.Invoke(this);
        }
    }
}