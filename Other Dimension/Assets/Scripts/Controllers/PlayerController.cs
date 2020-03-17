using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using GamePhysics;
using Menu;
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
        [SerializeField] private AudioSource _jumpAudio;
        [SerializeField] private AudioSource _dashAudio;
        [SerializeField] private AudioSource _bubbleAudio;
        [SerializeField] private Canvas _pauseMenu;
        [SerializeField] private MenuBehaviour _menuBehaviour;
        private IRayInteract _rayCube;
        private Vector3 _moveDirection = Vector3.zero;
        private Vector3 _directionVector = Vector3.zero;
        private Material Material => GetComponent<MeshRenderer>().material;
        private Transform RbTransform => _rb.transform;
        public Transform PlayerTransform => transform;
        public Rigidbody Rb => _rb;
        private bool _jumped;
        private bool _dashed;

        private void Awake()
        {
            _triggerSphere.radius = _interactDistance;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Cursor.visible = true;
                _pauseMenu.enabled = true;
                _menuBehaviour.PauseGame();
            }
            
            _moveDirection.x = Input.GetAxis("Horizontal");
            _moveDirection.z = Input.GetAxis("Vertical");
            float moveRotation = Input.GetAxis("Mouse X") * _rotateSpeed;
            transform.Rotate(0, moveRotation, 0);
            
            //Run
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                _walk *= _run;
            }
            //Stop running
            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                _walk /= _run;
            }
            //Dash
            if (Input.GetKeyDown(KeyCode.Space) && _jumped && !_dashed)
            {
                _rb.velocity = transform.forward * _jump;
                _dashAudio.Play();
                if (!_dashed) _dashed = true;
            }
            //Jump
            if (Input.GetKeyDown(KeyCode.Space) && !_jumped)
            {
                _rb.velocity = transform.up * _jump;
                _jumpAudio.Play();
                if (!_jumped) _jumped = true;
            }
            //Interact with cubes
            if (Input.GetKeyDown(KeyCode.F))
            {
                _rayCube?.RayInteraction(this);
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
            _bubbleAudio.Play();
            if (other.gameObject.layer == 9)
            {
                if (_jumped) _jumped = false;
                if (_dashed) _dashed = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            var rayComponent = other.GetComponent<IRayInteract>();
            if (rayComponent == null) return;
            if (_rayCube != null && _rayCube.FollowPlayer) return;
            _rayCube = rayComponent;
        }

        private void OnTriggerExit(Collider other)
        {
            var rayComponent = other.GetComponent<IRayInteract>();
            if (rayComponent == null) return;
            if (_rayCube == rayComponent) _rayCube = null;
        }

        public void DisplayColour(WhiteHole whiteHole, int current)
        {
            Material.SetColor("_BaseColor", whiteHole._colours[current]);
        }
    }
}
