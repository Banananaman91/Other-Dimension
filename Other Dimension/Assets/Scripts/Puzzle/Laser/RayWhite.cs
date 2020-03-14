using Controllers;
using UnityEngine;

namespace Puzzle.Laser
{
    public class RayWhite : RayMaster, IRayReceiver, IRayInteract
    {
        [SerializeField, Range(1, 10)] private int _distanceFromPlayer;
        [SerializeField] private int _followSpeed;
        [SerializeField] private int _rotateSpeed;
        private float _scrollScale = 1;
        private Vector3 _targetVector3;
        private bool _userLaserColourProperty;
        private PlayerController _player;
        private float _upDistance;
        public Color LaserColour { get; set; }
        public bool FollowPlayer { get; set; }
        public Transform Transform => transform;

        private void FixedUpdate()
        {
            if (FollowPlayer && _player)
            {
                var transform1 = _player.transform;
                var mouseScroll = Input.GetAxis("Mouse ScrollWheel");
                if (mouseScroll > 0) _upDistance += mouseScroll;
                else if (mouseScroll < 0) _upDistance += mouseScroll;
                _targetVector3 = transform1.position + transform1.forward * _distanceFromPlayer +
                                 transform1.up * _upDistance;
                transform.position = Vector3.Lerp(transform.position, _targetVector3, _followSpeed * Time.deltaTime);
                if (Input.GetKey(KeyCode.E))
                {
                    transform.Rotate(Vector3.up * _rotateSpeed * Time.deltaTime);
                }

                if (Input.GetKey(KeyCode.Q))
                {
                    transform.Rotate(-Vector3.up * _rotateSpeed * Time.deltaTime);
                }
            }
            
            if (Time.time > _rayRunOutTime)
            {
                _hitWithRay = false;
                NotHitWithRay();
            }

            var position = Transform.position;

            _transformDirection = transform.forward;
            position = Transform.position;
            _laserVisual.SetPosition(0, position);
            _laserVisual.SetPosition(1, position);

            if (!_hitWithRay)
            {
                return;
            }

            Physics.Raycast(position, _transformDirection, out _hit, Mathf.Infinity, -10);
            _laserVisual.SetPosition(1, position + _transformDirection * _distance);

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
            if (_rayReceiver == null) return;
            _rayReceiver.HitWithRay(this);

            if (!_addedColour)
            {
                _rayReceiver.LaserColour += _laserVisual.startColor;
                _addedColour = true;
            }
        }

        public void HitWithRay(Ray ray = null)
        {
            _hitWithRay = true;
            _rayRunOutTime = Time.time + _hitByRayRefreshTime;
            _laserVisual.startColor = LaserColour;
            _laserVisual.endColor = LaserColour;
            var laserParticleMain = _laserParticle.main;
            laserParticleMain.startColor = LaserColour;
        }

        public void NotHitWithRay()
        {
            _laserVisual.startColor = Color.black;
            _laserVisual.endColor = Color.black;
            var laserParticleMain = _laserParticle.main;
            laserParticleMain.startColor = Color.black;
            _laserVisual.SetPosition(1, Transform.position);
        }
        
        public void RayInteraction(PlayerController player)
        {
            _player = player;
            FollowPlayer = !FollowPlayer;
        }
    }
}
