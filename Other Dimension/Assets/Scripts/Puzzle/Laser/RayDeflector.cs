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
        private Vector3 _targetVector3;
        private bool _followPlayer;
        private Color _laserColour;
        private bool _userLaserColourProperty;
        private PlayerController _player;

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
            if (!_followPlayer || !_player) return;
            var transform1 = _player.transform;
            _targetVector3 = transform1.position + transform1.forward * _distanceFromPlayer;
            transform.position = Vector3.Lerp (transform.position, _targetVector3, _followSpeed * Time.deltaTime);
        }

        public new void HitWithRay()
        {
            if (_userLaserColourProperty) _laserColour = LaserColour;
            _hitWithRay = true;
            _rayRunOutTime = Time.time + _hitByRayRefreshTime;
            _laserVisual.startColor = _laserColour;
            _laserVisual.endColor = _laserColour;
            var laserParticleMain = _laserParticle.main;
            laserParticleMain.startColor = _laserColour;
        }

        public void RayInteraction(PlayerController player)
        {
            Debug.Log("Interacted");
            _player = player;
            if (!_followPlayer) _followPlayer = true;
            else if (_followPlayer) _followPlayer = false;
        }
    }
}
