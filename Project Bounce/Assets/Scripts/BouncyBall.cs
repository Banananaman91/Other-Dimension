using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

[RequireComponent(typeof(Rigidbody))]
public class BouncyBall : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _bounce;
    [SerializeField] private float _directionForce;
    [SerializeField] private float _rotationForce;
    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _rotationDirection = Vector3.zero;
    private Vector3 _directionVector = Vector3.zero;
    

    private void Start()
    {
        
    }

    private void Update()
    {
        _moveDirection = Vector3.zero;
        _rotationDirection = Vector3.zero;
        _moveDirection.x = Input.GetAxis("Horizontal");
        _moveDirection.z = Input.GetAxis("Vertical");
        if (_moveDirection != Vector3.zero) transform.forward = _moveDirection;
        
        if (Input.GetKeyDown(KeyCode.Space)) _rb.AddForce(Vector3.down * Mathf.Sqrt(_bounce * -2f * Physics.gravity.y), ForceMode.VelocityChange);
    }

    private void FixedUpdate()
    {
        _directionVector = (_rb.transform.right * _moveDirection.x) + (_rb.transform.forward * _moveDirection.z);
        _rb.MovePosition(_rb.position + Time.fixedDeltaTime * _directionForce * _directionVector);
//        Quaternion deltaRotation = Quaternion.Euler(_rotationForce * Time.deltaTime * _rotationDirection);
//        _rb.MoveRotation(_rb.rotation * deltaRotation);
    }
}
