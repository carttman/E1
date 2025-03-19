using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
    private Tower _tower;
    private Renderer _renderer;
    [SerializeField, Range(0.1f, 1f)] private float yScale = 0.2f;
    
    private void Awake()
    {
        _tower = GetComponentInParent<Tower>();
        _renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        //Debug.Log(_tower.TargetingRange);
        transform.localScale = new Vector3(_tower.TargetingRange * 2, yScale, _tower.TargetingRange * 2);
    }
}
