using UnityEngine;

public class Explosion : MonoBehaviour
{
    public delegate void CollideHandler(Collider collider);

    [SerializeField] private float duration = 0.5f;
    [SerializeField] private float radius = 1f;

    [SerializeField] private AnimationCurve opacityCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

    private float _age;
    private bool _effectOnly = true;
    private MeshRenderer _meshRenderer;
    private Color _startColor = Color.clear;
    public CollideHandler OnCollideDetected;

    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        if (_startColor == Color.clear) _startColor = _meshRenderer.material.color;
        transform.localScale = new Vector3(radius * 2f, radius * 2f, radius * 2f);

        Detonate();
    }

    private void Update()
    {
        _age += Time.deltaTime;
        if (_age >= duration) Destroy(gameObject);

        var t = _age / duration;
        _startColor.a = opacityCurve.Evaluate(t);
        _meshRenderer.material.color = _startColor;
        transform.localScale = Vector3.one * (scaleCurve.Evaluate(t) * radius * 2f);
    }

    private void OnDestroy()
    {
        OnCollideDetected = null;
    }

    public void Initialize(float inDuration, float inRadius, Color startColor, bool inEffectOnly = true)
    {
        duration = inDuration;
        radius = inRadius;
        _startColor = startColor;
        _effectOnly = inEffectOnly;
    }

    private void Detonate()
    {
        if (_effectOnly) return;

        var hitBuffer = new RaycastHit[100];

        var size = Physics.SphereCastNonAlloc(
            transform.position,
            radius,
            Vector3.up,
            hitBuffer,
            radius);
        if (size <= 0) return;
        for (var i = 0; i < size; i++)
            //Debug.Log("invoking detonate...");
            OnCollideDetected?.Invoke(hitBuffer[i].collider);
        //Debug.Log(OnCollideDetected);
    }
}