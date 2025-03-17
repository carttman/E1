
using UnityEngine;
using RaycastHit = UnityEngine.RaycastHit;

public class Shell : MonoBehaviour
{
    private Vector3 launchPoint;
    private Vector3 targetPoint;
    private Vector3 launchVelocity;

    public float blastRadius = 5f;
    public float damage = 100f;

    private float age;

    public void Initialize(Vector3 launchPoint, Vector3 targetPoint, Vector3 launchVelocity)
    {
        this.launchPoint = launchPoint;
        this.targetPoint = targetPoint;
        this.launchVelocity = launchVelocity;
    }

    private void Update()
    {
        age += Time.deltaTime;
        Vector3 p = launchPoint + launchVelocity * age;
        p.y -= 0.5f * 9.81f * age * age;
        transform.localPosition = p;

        if (p.y < 0f)
        {
            RaycastHit[] hits = new RaycastHit[100];
            
            var size = Physics.SphereCastNonAlloc(origin: transform.position, radius: blastRadius, direction: Vector3.up, hits);
            if (size > 0)
            {
                for (int i = 0; i < size; i++)
                {
                    GameObject go = hits[i].transform.gameObject;
                    go.GetComponent<StatsComponent>()?.TakeDamage(damage);
                }
            }
            
            Destroy(gameObject);
        }

        Vector3 d = launchVelocity;
        d.y -= 9.81f * age;
        transform.localRotation = Quaternion.LookRotation(d);
    }
}
