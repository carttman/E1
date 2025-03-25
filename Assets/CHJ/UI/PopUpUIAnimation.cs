using TMPro;
using UnityEngine;

public class PopUpUIAnimation : MonoBehaviour
{
    [Header("AnimationCurve")]
    public AnimationCurve opacityCurve;
    public AnimationCurve scaleCurve;
    public AnimationCurve heightCurve;
    
    private TextMeshProUGUI tmp;
    private float time = 0;
    private Vector3 origin;
    
    void Start()
    {
        tmp = transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        origin = transform.position;
    }

    void Update()
    {
        UpdateTextAnimation();
    }

    void UpdateTextAnimation()
    {
        tmp.color = new Color(tmp.color.r, tmp.color.g, tmp.color.b, opacityCurve.Evaluate(time));
        transform.localScale = Vector3.one * scaleCurve.Evaluate(time);
        transform.position = origin + new Vector3(0, heightCurve.Evaluate(time) + 1, 0);
        time += Time.deltaTime;
    }
}
