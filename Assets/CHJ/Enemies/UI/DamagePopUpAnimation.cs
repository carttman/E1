using TMPro;
using UnityEngine;

public class DamagePopUpAnimation : MonoBehaviour
{
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
        tmp.color = new Color(1, 1, 1, opacityCurve.Evaluate(time));
        transform.localScale = Vector3.one * scaleCurve.Evaluate(time);
        transform.position = origin + new Vector3(0, heightCurve.Evaluate(time) + 1, 0);
        time += Time.deltaTime;
    }
}
