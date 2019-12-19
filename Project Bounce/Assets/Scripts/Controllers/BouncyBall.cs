using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Controllers
{
    [RequireComponent(typeof(Rigidbody))]
    public class BouncyBall : MonoBehaviour
    {
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private Camera _camera;
        [SerializeField] private float _bounce;
        [SerializeField] private float _directionForce;
        [SerializeField] private GameObject _antiBlackHole;
        [SerializeField] private int _rotateSpeed;

        private Vector3 _moveDirection = Vector3.zero;
        private Vector3 _directionVector = Vector3.zero;
        private Transform RbTransform => _rb.transform;
        public Rigidbody Rb => _rb;
        private Vector3 Direction => _antiBlackHole.transform.position - transform.position;
        private float _gravity = 9.08f;

        private void Update()
        {
            _moveDirection = Vector3.zero;
            _moveDirection.x = Input.GetAxis("Horizontal");
            _moveDirection.z = Input.GetAxis("Vertical");
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _rb.AddForce(Vector3.down * Mathf.Sqrt(_bounce * -2f * Physics.gravity.y), ForceMode.VelocityChange);
            }
            //transform.forward = new Vector3(0, 0, Input.GetAxis("Mouse X") * _rotateSpeed);
        }

        private void FixedUpdate()
        {
            transform.up = Direction;
            //transform.Rotate(0, Input.GetAxis("Mouse X") * _rotateSpeed, 0, Space.Self);
            _directionVector = (RbTransform.right * _moveDirection.x) + (RbTransform.forward * _moveDirection.z);
            _rb.MovePosition(RbTransform.position + Time.fixedDeltaTime * _directionForce * _directionVector);
            ApplyGravity();
        }

        private void ApplyGravity()
        {
            _rb.AddForce(-Direction * (_gravity / 60));
            //_rb.AddForce(-transform.up * _gravity);
        }
    }
}
