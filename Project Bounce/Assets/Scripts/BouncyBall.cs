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
    [SerializeField] private Terrain _terrain;
    [SerializeField] private float _bounce;
    [SerializeField] private float _directionForce;
    [SerializeField] private float _jumpHeight;
    private Vector3 _moveDirection = Vector3.zero;
    private Vector3 _directionVector = Vector3.zero;
    
    private Transform RB => _rb.transform;
    private PhysicMaterial Bounce => GetComponent<SphereCollider>().material;

    private void Update()
    {
        _moveDirection = Vector3.zero;
        _moveDirection.x = Input.GetAxis("Horizontal");
        _moveDirection.z = Input.GetAxis("Vertical");
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _rb.AddForce(Vector3.down * Mathf.Sqrt(_bounce * -2f * Physics.gravity.y), ForceMode.VelocityChange);
            Debug.Log(Vector3.Distance(_terrain.transform.position, transform.position));
        }
//        if (Vector3.Distance(_terrain.transform.position, transform.position) > _jumpHeight)
//        {
//            Bounce.bounceCombine = PhysicMaterialCombine.Minimum;
//        }
//        else
//        {
//            Bounce.bounceCombine = PhysicMaterialCombine.Maximum;
//        }
        transform.rotation = Quaternion.Euler(0, _camera.transform.eulerAngles.y, 0);

    }

    private void FixedUpdate()
    {
        _directionVector = (RB.right * _moveDirection.x) + (RB.forward * _moveDirection.z);
        _rb.MovePosition(RB.position + Time.fixedDeltaTime * _directionForce * _directionVector);
        
    }
}
