using System;
using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    private Vector3 inputVector;
    [SerializeField]
    private float moveSpeed = 5.0f;

    private void Update()
    {
        inputVector = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        inputVector.Normalize();
        
        if (inputVector != Vector3.zero)
        {
            transform.Translate(inputVector * (moveSpeed * Time.deltaTime));
        }
    }
}
