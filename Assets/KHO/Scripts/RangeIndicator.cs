using UnityEngine;

public class RangeIndicator : MonoBehaviour
{
    [SerializeField] [Range(0.1f, 1f)] private float yScale = 0.2f;
    // 적 충돌이 구형이므로 더해줌
    [SerializeField] private float plusVisualRange = 0.225f;
    private Renderer _renderer;
    private Tower _tower;

    private void Awake()
    {
        _tower = GetComponentInParent<Tower>();
        _renderer = GetComponent<Renderer>();
    }

    private void Start()
    {
        UpdateRange();
    }

    private void UpdateRange()
    {
        transform.localScale = new Vector3(_tower.TargetingRange * 2 + plusVisualRange, yScale,
            _tower.TargetingRange * 2 + plusVisualRange);
    }
}