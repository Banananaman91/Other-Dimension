using System;
using System.Numerics;
using Puzzle.Laser;
using UnityEngine;
using UnityEngine.UIElements;
using Cursor = UnityEngine.Cursor;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Player")]
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private float _walk;
        [SerializeField, Range(1,5)] private int _run;
        [SerializeField] private int _rotateSpeed;
        [SerializeField] private int _jump;
        [SerializeField] private int _dash;
        [SerializeField] private float _maxSpeed;
        [SerializeField] private int _interactDistance;
        [SerializeField] private SphereCollider _triggerSphere;
        private RayDeflector _rayCube;
        private Vector3 _moveDirection = Vector3.zero;
        private Vector3 _directionVector = Vector3.zero;
        private Transform RbTransform => _rb.transform;
        public Transform PlayerTransform => transform;
        public Rigidbody Rb => _rb;
        private bool _jumped;
        private bool _dashed;

        private void Awake()
        {
            _triggerSphere.radius = _interactDistance;
            Cursor.visible = false;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape)) Cursor.visible = true;
            _moveDirection.x = Input.GetAxis("Horizontal");
            _moveDirection.z = Input.GetAxis("Vertical");
            float moveRotation = Input.GetAxis("Mouse X") * _rotateSpeed;
            transform.Rotate(0, moveRotation, 0);

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
                _rb.velocity = transform.forward * _jump;
                if (!_dashed) _dashed = true;
            }
            if (Input.GetKeyDown(KeyCode.Space) && !_jumped)
            {
                _rb.velocity = transform.up * _jump;
                if (!_jumped) _jumped = true;
            }
            if (Input.GetKey(KeyCode.F))
            {
                _rayCube.RayInteraction(this);
            }
        }

        private void FixedUpdate()
        {
            _rb.velocity = Vector3.ClampMagnitude(_rb.velocity, _maxSpeed);
            _directionVector = (RbTransform.right * _moveDirection.x) + (RbTransform.forward * _moveDirection.z);
            _rb.MovePosition(RbTransform.position + _directionVector * _walk * Time.deltaTime);
        }

        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.layer == 9)
            {
                if (_jumped) _jumped = false;
                if (_dashed) _dashed = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var rayComponent = other.GetComponent<RayDeflector>();
            if (!rayComponent) return;
            if (_rayCube && _rayCube._followPlayer) return;
            _rayCube = rayComponent;
        }
    }
}
