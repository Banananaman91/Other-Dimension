using System;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle.Laser
{
    public class RayGoal : MonoBehaviour, IRayReceiver
    {
        [SerializeField] private LineRenderer _laserVisual;
        [SerializeField] private ParticleSystem _laserParticle;
        private float _distance = 100;
        private RaycastHit _hit;
        private bool _hitWithRay;
        private float _rayRunOutTime;
        private float _hitByRayRefreshTime = 1f;
        public Color LaserColour { get; set; }

        private void Awake()
        {
            _laserVisual.startColor = Color.black;
            _laserVisual.endColor = Color.black;
            var laserParticleMain = _laserParticle.main;
            laserParticleMain.startColor = Color.black;
            _laserVisual.SetPosition(1, new Vector3(0, 0, 0));
        }

        public void HitWithRay()
        {
            _hitWithRay = true;
            _rayRunOutTime = Time.time + _hitByRayRefreshTime;
        }

        public void NotHitWithRay()
        {
            _laserVisual.startColor = Color.black;
            _laserVisual.endColor = Color.black;
            var laserParticleMain = _laserParticle.main;
            laserParticleMain.startColor = Color.black;
            _laserVisual.SetPosition(1, new Vector3(0, 0, 0));
        }
        
        private void FixedUpdate()
        {
            if (Time.time > _rayRunOutTime)
            {
                _hitWithRay = false;
                NotHitWithRay();
            }
            if (!_hitWithRay) return;
            _laserVisual.startColor = LaserColour;
            _laserVisual.endColor = LaserColour;
            var laserParticleMain = _laserParticle.main;
            laserParticleMain.startColor = LaserColour;
            Physics.Raycast(transform.position, transform.TransformDirection(transform.up), out _hit, _distance);
            _laserVisual.SetPosition(1, new Vector3(0, _distance, 0));
            if (!_hit.collider) return;
            var distance = Vector3.Distance(transform.position, _hit.point);
            _laserVisual.SetPosition(1, new Vector3(0, distance * 2, 0));
            var rayReceiver = _hit.collider.gameObject.GetComponent<IRayReceiver>();
            rayReceiver?.HitWithRay();
        }
    }
}
