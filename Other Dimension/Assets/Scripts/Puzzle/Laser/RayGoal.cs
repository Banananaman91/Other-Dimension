using System;
using System.Collections.Generic;
using Controllers;
using Puzzle.Builder;
using UnityEngine;

namespace Puzzle.Laser
{
    public class RayGoal : RayMaster, IRayInteract, IRayReceiver
    {
        [SerializeField] private GoalState _currentState;
        [SerializeField] private AudioSource _audio;
        private Color _finalColour;
        private bool _freezeColour;
        private Ray _ray;
        public Color LaserColour { get; set; }
        public Transform Transform => transform;
        private Material _material => GetComponent<MeshRenderer>().material;

        private void Update()
        {
            switch (_currentState)
            {
                case GoalState.Awaiting:
                    if (_audio.isPlaying) _audio.Stop();
                    break;
                case GoalState.Fired:
                    Fired();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
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
            if (!_hitWithRay && !_goalActive)
            {
                _laserVisual.SetPosition(0, position);
                _laserVisual.SetPosition(1, position);
                return;
            }

            _transformDirection = Vector3.zero - Transform.position;
            if (!_goalActive) return;
            Physics.Raycast(position, _transformDirection, out _hit, Mathf.Infinity);
            if (!_hit.collider) return;
            _laserVisual.SetPosition(1, _hit.point);
            _rayReceiver = _hit.collider.gameObject.GetComponent<IRayReceiver>();
            _rayReceiver?.HitWithRay(this);
        }

        private void Fired()
        {
            if (!_audio.isPlaying) _audio.Play();
            if (!_freezeColour && _finalColour != LaserColour) {
                _finalColour = LaserColour;
                _freezeColour = true;
            }
            _laserVisual.startColor = _finalColour;
            _laserVisual.endColor = _finalColour;
            var laserParticleMain = _laserParticle.main;
            laserParticleMain.startColor = _finalColour;
            _material.SetColor("_BaseColor", _finalColour);
        }

        public bool FollowPlayer { get; set; }

        public void RayInteraction(PlayerController player)
        {
            Debug.Log("Goal activated");
            _goalActive = true;
            _currentState = GoalState.Fired;
        }
        
        public void HitWithRay(Ray ray)
        {
            _ray = ray;
            _hitWithRay = true;
            _rayRunOutTime = Time.time + _hitByRayRefreshTime;
            _laserVisual.startColor = LaserColour;
            _laserVisual.endColor = LaserColour;
            var laserParticleMain = _laserParticle.main;
            laserParticleMain.startColor = LaserColour;
        }

        public void NotHitWithRay()
        {
            if(_ray) _ray._addedColour = false;
            _laserVisual.startColor = Color.black;
            _laserVisual.endColor = Color.black;
            var laserParticleMain = _laserParticle.main;
            laserParticleMain.startColor = Color.black;
            _laserVisual.SetPosition(1, Transform.position);
        }
    }
}
