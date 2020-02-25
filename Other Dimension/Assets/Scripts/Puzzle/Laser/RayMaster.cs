using UnityEngine;

namespace Puzzle.Laser
{
    public class RayMaster : Ray, IRayReceiver
    {
        [SerializeField] private Transform _parentTransform;
        public Color LaserColour { get; set; }

        private void Awake()
        {
            switch (_directionType)
            {
                case Direction.Forward:
                    _transformDirection = transform.forward;
                    break;
                case Direction.Up:
                    _transformDirection = _whiteHole - transform.position;
                    break;
            }
            _laserVisual.startColor = Color.black;
            _laserVisual.endColor = Color.black;
            var laserParticleMain = _laserParticle.main;
            laserParticleMain.startColor = Color.black;
            _laserVisual.SetPosition(1, new Vector3(0, 0, 0));
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
            Physics.Raycast(transform.position, transform.TransformDirection(_transformDirection), out _hit, _distance);
            Debug.DrawRay(transform.position, _transformDirection, Color.green);
            _laserVisual.SetPosition(1, _transformDirection * _distance);
            if (!_hit.collider) return;
            var distance = Vector3.Distance(transform.position, _hit.point);
            _laserVisual.SetPosition(1, _hit.point);
            if (gameObject.name == "Sphere") Debug.Log(_hit.collider.gameObject.name + ": " + _hit.transform.position);
            var rayReceiver = _hit.collider.gameObject.GetComponent<IRayReceiver>();
            rayReceiver?.HitWithRay(this);
        }
    }
}
