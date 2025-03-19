using UnityEngine;

public class MortarTower : Tower
{
    [SerializeField] private float shotsPerSecond = 1f;
    [SerializeField] private Transform mortar;
    [SerializeField] private GameObject shellPrefab;
    private float _launchSpeed;
    private float _launchProgress;

    private new void Awake()
    {
        base.Awake();
        OnValidate();
    }

    private void OnValidate()
    {
        float x = -targetingRange * transform.localScale.x + 0.25001f;
        float y = -mortar.position.y;
        _launchSpeed = Mathf.Sqrt(9.81f * (y + Mathf.Sqrt(x * x + y * y)));
    }

    private void Update()
    {
        _launchProgress += shotsPerSecond * Time.deltaTime;
        while (_launchProgress >= 1f)
        {
            if (AcquireTarget(out Transform pTarget))
            {
                if (pTarget)
                {
                    TrackTarget(ref pTarget);
                    Launch(pTarget);
                }
                _launchProgress -= 1f;
            }
            else
            {
                _launchProgress = 0.999f;
            }
        }
    }

    private void Launch(Transform target)
    {
        Vector3 launchPoint = mortar.position;
        Vector3 targetPoint = target.position;
        targetPoint.y = 0f;

        Vector2 dir;
        dir.x = targetPoint.x - launchPoint.x;
        dir.y = targetPoint.z - launchPoint.z;
        float x = dir.magnitude;
        float y = -launchPoint.y;
        dir /= x;
        
        float g = 9.81f;
        float s = _launchSpeed;
        float s2 = s * s;

        float r = s2 * s2 - g * (g * x * x + 2f * y * s2);

        if (r < 0f)
        {
            //Debug.Log("Launch velocity insufficient for range!");
            return;
        }
        
        float tanTheta = (s2 + Mathf.Sqrt(r)) / (g * x);
        float cosTheta = Mathf.Cos(Mathf.Atan(tanTheta));
        float sinTheta = cosTheta * tanTheta;

        mortar.localRotation = Quaternion.LookRotation(new Vector3(dir.x, tanTheta, dir.y));
        GameObject shell = Instantiate(shellPrefab);
        Shell shellScript = shell.GetComponent<Shell>();
        shellScript.Initialize(launchPoint, targetPoint, 
            new Vector3(s * cosTheta * dir.x, s * sinTheta, s * cosTheta * dir.y), this);
        
        /*
        // 경로 그리기
        Vector3 prev = launchPoint;
        Vector3 next;

        for (int i = 1; i <= 18; i++)
        {
            // 3초간의 경로
            float t = i / 6f;
            float dx = s * cosTheta * t;
            float dy = s * sinTheta * t - 0.5f * g * t * t;
            next = launchPoint + new Vector3(dir.x * dx, dy, dir.y * dx);
            Debug.DrawLine(prev, next, Color.blue, 1f);
            prev = next;
        }
        
        Debug.DrawLine(launchPoint, targetPoint, Color.yellow, 1f);
        Debug.DrawLine(
            new Vector3(launchPoint.x, 0.01f, launchPoint.z),
            new Vector3(launchPoint.x + dir.x * x, 0.01f, launchPoint.z + dir.y * x),
            Color.white,
            duration: 1f);
        */
    }
}
