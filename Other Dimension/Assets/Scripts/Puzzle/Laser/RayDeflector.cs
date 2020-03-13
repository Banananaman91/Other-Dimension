using System;
using Controllers;
using UnityEngine;

namespace Puzzle.Laser
{
    public class RayDeflector : RayMaster, IRayReceiver, IRayInteract
    {
        [SerializeField] private CubeColour _colourType;
        [SerializeField] private Material _material;
        [SerializeField, Range(1, 10)] private int _distanceFromPlayer;
        [SerializeField] private int _followSpeed;
        [SerializeField] private int _rotateSpeed;
        private float _scrollScale = 1;
        private Vector3 _targetVector3;
        private Color _laserColour;
        private bool _userLaserColourProperty;
        private PlayerController _player;
        private float _upDistance;


        private void Start()
        {
            switch (_colourType)
            {
                case CubeColour.Blue:
                    _laserColour = Color.blue;
                    _material.color = _laserColour;
                    break;
                case CubeColour.Green:
                    _laserColour = Color.green;
                    _material.color = _laserColour;
                    break;
                case CubeColour.Red:
                    _laserColour = Color.red;
                    _material.color = _laserColour;
                    break;
                case CubeColour.White:
                    _userLaserColourProperty = true;
                    break;

            }
        }

        private void FixedUpdate()
        {
            if (Time.time > _rayRunOutTime)
            {
                _hitWithRay = false;
                NotHitWithRay();
            }
            var position = Transform.position;
            switch (_directionType)
            {
                case Direction.Forward:
                    
                    _transformDirection = transform.forward;
                    position = Transform.position;
                    _laserVisual.SetPosition(0, position);
                    _laserVisual.SetPosition(1, position);
                    
                    if (!_hitWithRay)
                    {
                        break;
                    }
                    
                    Physics.Raycast(position, _transformDirection, out _hit, Mathf.Infinity, -10);
                    
                    if (!_hit.collider)
                    {
                        if (_rayReceiver == null) return;
                        _rayReceiver.LaserColour -= _laserVisual.startColor;
                        _rayReceiver = null;
                        _addedColour = false;
                        return;
                    }
                    
                    _laserVisual.SetPosition(1, _hit.point);
                    _rayReceiver = _hit.collider.gameObject.GetComponent<IRayReceiver>();
                    if (_rayReceiver == null) break;
                    _rayReceiver.HitWithRay(this);
                    
                    if (!_addedColour)
                    {
                        _rayReceiver.LaserColour += _laserVisual.startColor;
                        _addedColour = true;
                    }
                    
                    break;
                case Direction.Up:
                    
                    _transformDirection = Vector3.zero - Transform.position;
                    
                    if (_goalActive) return;
                    
                    Physics.Raycast(position, _transformDirection, out _hit, Mathf.Infinity -10);
                    
                    if (!_hit.collider) return;
                    
                    _laserVisual.SetPosition(1, _hit.point);
                    _rayReceiver = _hit.collider.gameObject.GetComponent<IRayReceiver>();
                    _rayReceiver?.HitWithRay(this);
                    
                    break;
            }
            
            if (!FollowPlayer || !_player) return;
            
            var transform1 = _player.transform;
            var mouseScroll = Input.GetAxis("Mouse ScrollWheel");
            if (mouseScroll > 0) _upDistance += mouseScroll;
            else if (mouseScroll < 0) _upDistance += mouseScroll;
            _targetVector3 = transform1.position + transform1.forward * _distanceFromPlayer + transform1.up * _upDistance;
            transform.position = Vector3.Lerp (transform.position, _targetVector3, _followSpeed * Time.deltaTime);
            
            if(Input.GetKey (KeyCode.E))
            {
                transform.Rotate(Vector3.up * _rotateSpeed * Time.deltaTime);
            }

            if(Input.GetKey (KeyCode.Q))
            {
                transform.Rotate(-Vector3.up * _rotateSpeed * Time.deltaTime);
            }
        }

        public void HitWithRay(Ray ray = null)
        {
            if (_userLaserColourProperty) _laserColour = LaserColour;
            _hitWithRay = true;
            _rayRunOutTime = Time.time + _hitByRayRefreshTime;
            _laserVisual.startColor = _laserColour;
            _laserVisual.endColor = _laserColour;
            var laserParticleMain = _laserParticle.main;
            laserParticleMain.startColor = _laserColour;
        }

        public void NotHitWithRay()
        {
            _laserVisual.startColor = Color.black;
            _laserVisual.endColor = Color.black;
            var laserParticleMain = _laserParticle.main;
            laserParticleMain.startColor = Color.black;
            _laserVisual.SetPosition(1, Transform.position);
        }

        public bool FollowPlayer { get; set; }

        public void RayInteraction(PlayerController player)
        {
            Debug.Log("Interacted");
            _player = player;
            if (!FollowPlayer) FollowPlayer = true;
            else if (FollowPlayer) FollowPlayer = false;
        }
    }
}
