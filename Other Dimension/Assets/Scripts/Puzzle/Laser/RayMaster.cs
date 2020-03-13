using UnityEngine;

namespace Puzzle.Laser
{
    public class RayMaster : Ray
    {
        protected Transform Transform => transform;
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
    }
}
