using UnityEngine;

namespace Puzzle.Laser
{
    public class RayMaster : Ray, IRayReceiver
    {
        private Transform Transform => transform;
        protected bool _goalActive;
        public Color LaserColour { get; set; }

        private void Awake()
        {
            switch (_directionType)
            {
                case Direction.Forward:
                    _transformDirection = Transform.forward;
                    break;
                case Direction.Up:
                    _transformDirection = Vector3.zero - Transform.position;
                    break;
            }
            _laserVisual.startColor = Color.black;
            _laserVisual.endColor = Color.black;
            var laserParticleMain = _laserParticle.main;
            laserParticleMain.startColor = Color.black;
            var position = Transform.position;
            _laserVisual.SetPosition(0, position);
            _laserVisual.SetPosition(1, position);
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
    }
}
