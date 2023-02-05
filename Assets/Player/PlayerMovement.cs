using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RigidbodyBuoyancy))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private RigidbodyBuoyancy _rigidbodyBuoyancy;
    [SerializeField] private Rigidbody _rb;

    [SerializeField] private float _forwardSpeed;
    [SerializeField] private float _turnSpeed;
    // Start is called before the first frame update
    void Start()
    {
        if (_rb == null)
        {
            _rb = gameObject.GetComponent<Rigidbody>();
        }
        if (_rb == null)
        {
            Debug.LogError("PlayerMovement: Rigidbody component is null!");
        }
        if (_rigidbodyBuoyancy == null)
        {
            _rigidbodyBuoyancy = gameObject.GetComponent<RigidbodyBuoyancy>();
        }
        if (_rigidbodyBuoyancy == null)
        {
            Debug.LogError("PlayerMovement: RigidbodyBuoyancy script is null!");
        }
    }

    void FixedUpdate()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (_rigidbodyBuoyancy.isUnderwater)
        {
            _rb.AddRelativeForce(Vector3.forward * _forwardSpeed);
            _rb.AddRelativeTorque(Vector3.up * horizontalInput * _turnSpeed);
        }    
    }
}
