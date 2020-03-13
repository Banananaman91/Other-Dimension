using System;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

namespace Puzzle.Laser
{
    public class RayGoal : RayMaster, IRayInteract, IRayReceiver
    {
        [SerializeField] private GoalState _currentState;
        private Color _finalColour;
        private bool _freezeColour;

        private void Update()
        {
            switch (_currentState)
            {
                case GoalState.Awaiting:
                    break;
                case GoalState.Fired:
                    Fired();
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
            if (!_hitWithRay)
            {
                _laserVisual.SetPosition(0, position);
                _laserVisual.SetPosition(1, position);
                return;
            }
            switch (_directionType)
            {
                case Direction.Forward:
                    _transformDirection = transform.forward;
                    position = Transform.position;
                    _laserVisual.SetPosition(0, position);
                    _laserVisual.SetPosition(1, position);
                    Physics.Raycast(position, _transformDirection, out _hit, Mathf.Infinity);
                    if (!_hit.collider) return;
                    _laserVisual.SetPosition(1, _hit.point);
                    var rayReceiver = _hit.collider.gameObject.GetComponent<IRayReceiver>();
                    rayReceiver?.HitWithRay(this);
                    break;
                case Direction.Up:
                    _transformDirection = Vector3.zero - Transform.position;
                    if (_goalActive) return;
                    Physics.Raycast(position, _transformDirection, out _hit, Mathf.Infinity);
                    if (!_hit.collider) return;
                    _laserVisual.SetPosition(1, _hit.point);
                    rayReceiver = _hit.collider.gameObject.GetComponent<IRayReceiver>();
                    rayReceiver?.HitWithRay(this);
                    break;
            }
        }

        private void Fired()
        {
            if (!_freezeColour && _finalColour != LaserColour) {
                _finalColour = LaserColour;
                _freezeColour = true;
            }
            _laserVisual.startColor = _finalColour;
            _laserVisual.endColor = _finalColour;
            var laserParticleMain = _laserParticle.main;
            laserParticleMain.startColor = _finalColour;
        }

        public void RayInteraction(PlayerController player)
        {
            _goalActive = true;
            _currentState = GoalState.Fired;
        }
        
        public void HitWithRay(Ray ray)
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
    }
}
