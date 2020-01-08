using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private float _walk;
        [SerializeField, Range(1,5)] private int _run;
        [SerializeField] private int _rotateSpeed;
        [SerializeField] private int _jump;
        [SerializeField] private int _dash;
        private Vector3 _moveDirection = Vector3.zero;
        private Vector3 _directionVector = Vector3.zero;
        private Transform RbTransform => _rb.transform;
        public Transform PlayerTransform => transform;
        public Rigidbody Rb => _rb;
        private bool _jumped;
        private bool _dashed;

        private void Update()
        {
            _moveDirection.x = Input.GetAxis("Horizontal");
            _moveDirection.z = Input.GetAxis("Vertical");

            if(Input.GetKey (KeyCode.E))
            {
                transform.Rotate(Vector3.up * _rotateSpeed * Time.deltaTime);
            }

            if(Input.GetKey (KeyCode.Q))
            {
                transform.Rotate(-Vector3.up * _rotateSpeed * Time.deltaTime);
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _walk *= _run;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _walk /= _run;
            }
            if (Input.GetKeyDown(KeyCode.Space) && _jumped && !_dashed)
            {
                _rb.AddForce(transform.forward * _dash, ForceMode.Impulse);
                if (!_dashed) _dashed = true;
            }
            if (Input.GetKeyDown(KeyCode.Space) && !_jumped)
            {
                _rb.AddForce(transform.up * _jump, ForceMode.Impulse);
                if (!_jumped) _jumped = true;
            }
        }

        private void FixedUpdate()
        {
            _directionVector = (RbTransform.right * _moveDirection.x) + (RbTransform.forward * _moveDirection.z);
            _rb.MovePosition(RbTransform.position + _directionVector * _walk * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                if (_jumped) _jumped = false;
                if (_dashed) _dashed = false;
            }
        }
    }
}
