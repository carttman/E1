using System;
using UnityEngine;

public class AoeEffect : MonoBehaviour
{
    [SerializeField] private float duration = 0.5f;
    private float age;
    
    [SerializeField] private float radius = 1f;
    [SerializeField] private AnimationCurve opacityCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);
    [SerializeField] private AnimationCurve scaleCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

    private Color _startColor;
    private MeshRenderer _meshRenderer;
    
    public void Initialize(float duration, float radius)
    {
        this.duration = duration;
        this.radius = radius;
    }
    
    private void Start()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
        _startColor = _meshRenderer.material.color;
        transform.localScale = new Vector3(radius * 2f, radius * 2f, radius * 2f);
    }

    private void Update()
    {
        age += Time.deltaTime;
        if (age >= duration)
        {
            Destroy(gameObject);
        }
        
        float t = age / duration;
        _startColor.a = opacityCurve.Evaluate(t);
        _meshRenderer.material.color = _startColor;
        transform.localScale = Vector3.one * (scaleCurve.Evaluate(t) * radius * 2f);
    }
}
