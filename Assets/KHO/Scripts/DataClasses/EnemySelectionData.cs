using UnityEngine;

public class EnemySelectionData : SelectionData
{
    public Global.Element Element;
    
    private string _name;
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
    
    private float _maxHealth;
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

    private float _health;
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

    private float _moveSpeed;

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
