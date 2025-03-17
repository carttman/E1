using System;
using UnityEngine;

public class Shell : MonoBehaviour
{
    private Vector3 launchPoint;
    private Vector3 targetPoint;
    private Vector3 launchVelocity;

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
            Destroy(gameObject);
        }

        Vector3 d = launchVelocity;
        d.y -= 9.81f * age;
        transform.localRotation = Quaternion.LookRotation(d);
    }
}
