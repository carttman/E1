using UnityEngine;

public class LightRotator : MonoBehaviour
{
    [SerializeField] float rotateSpeed = 10f;
    [SerializeField] Vector3 rotateAxis = Vector3.right;
    
    void Update()
    {
        transform.rotation *= Quaternion.AngleAxis(rotateSpeed * Time.deltaTime, rotateAxis);
    }
}
