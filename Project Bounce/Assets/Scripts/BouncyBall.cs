using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

[RequireComponent(typeof(Rigidbody))]
public class BouncyBall : MonoBehaviour
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private Camera _camera;
    [SerializeField] private float _bounce;
    [SerializeField] private float _directionForce;
    [SerializeField] private float clampDistance;
    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _directionVector = Vector3.zero;
    private Vector3 MyTransform => transform.position;
    private Vector3 CameraTransform => _camera.transform.position;
    private Transform RB => _rb.transform;

    private void Update()
    {
        _moveDirection = Vector3.zero;
        _moveDirection.x = Input.GetAxis("Horizontal");
        _moveDirection.z = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.Space)) _rb.AddForce(Vector3.down * Mathf.Sqrt(_bounce * -2f * Physics.gravity.y), ForceMode.VelocityChange);
        transform.rotation = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0);
        _camera.transform.position = MyTransform + ((MyTransform - CameraTransform).normalized * clampDistance);
    }

    private void FixedUpdate()
    {
        _directionVector = (RB.right * _moveDirection.x) + (RB.forward * _moveDirection.z);
        _rb.MovePosition(RB.position + Time.fixedDeltaTime * _directionForce * _directionVector);
    }
}
