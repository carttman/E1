using UnityEngine;

public class MortarTower : Tower
{
    [SerializeField] private float shotsPerSecond = 1f;
    [SerializeField] private Transform mortar;
    [SerializeField] private GameObject shellPrefab;
    [SerializeField] private float leadTime = 0.5f;
    [SerializeField] private float damage;
    private float _launchProgress = 0.999f;

    private float _launchSpeed;

    private new void Awake()
    {
        base.Awake();
        OnValidate();
    }

    private new void Start()
    {
        base.Start();
        OnRarityChanged();
    }

    private new void Update()
    {
        base.Update();
        
        _launchProgress += shotsPerSecond * Time.deltaTime;
        while (_launchProgress >= 1f)
            if (AcquireTarget(out var pTarget))
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

    private new void OnValidate()
    {
        base.OnValidate();
        var x = -targetingRange * transform.localScale.x + 0.25001f;
        var y = -mortar.position.y;
        _launchSpeed = Mathf.Sqrt(9.81f * (y + Mathf.Sqrt(x * x + y * y)));
    }

    protected override void OnRarityChanged()
    {
        damage = towerData.TowerStats[(int)Rarity].damage;
        shotsPerSecond = towerData.TowerStats[(int)Rarity].attackSpeed;
    }

    private void Launch(Transform target)
    {
        var launchPoint = mortar.position;
        var targetPoint = target.GetComponent<EnemyMove>().GetPredictedPosition(leadTime);
        targetPoint.y = 0f;

        Vector2 dir;
        dir.x = targetPoint.x - launchPoint.x;
        dir.y = targetPoint.z - launchPoint.z;
        var x = dir.magnitude;
        var y = -launchPoint.y;
        dir /= x;

        var g = 9.81f;
        var s = _launchSpeed;
        var s2 = s * s;

        var r = s2 * s2 - g * (g * x * x + 2f * y * s2);

        if (r < 0f)
            //Debug.Log("Launch velocity insufficient for range!");
            return;

        var tanTheta = (s2 + Mathf.Sqrt(r)) / (g * x);
        var cosTheta = Mathf.Cos(Mathf.Atan(tanTheta));
        var sinTheta = cosTheta * tanTheta;
        var launchVelocity = new Vector3(s * cosTheta * dir.x, s * sinTheta, s * cosTheta * dir.y);

        var damagePacket = new DamagePacket(damage, towerData.elementType, this);

        var shellGo = PoolManager.Instance.GetProjectile(
            shellPrefab,
            launchPoint,
            Quaternion.identity,
            null,
            damagePacket
        );

        var shell = shellGo.GetComponent<Shell>();
        shell.Initialize(launchPoint,
            targetPoint,
            launchVelocity
        );
        shell.gameObject.SetActive(true);
        AudioManager.instance.PlaySound(SoundEffect.ProjLaunch);

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