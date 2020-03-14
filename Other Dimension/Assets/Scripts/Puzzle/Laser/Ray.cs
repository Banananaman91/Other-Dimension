using GamePhysics;
using UnityEngine;

namespace Puzzle.Laser
{
    public class Ray : MonoBehaviour
    {
        [SerializeField] protected LineRenderer _laserVisual;
        [SerializeField] protected ParticleSystem _laserParticle;
        [SerializeField] protected Direction _directionType;
        protected Vector3 _transformDirection;
        protected float _distance = 100;
        public RaycastHit _hit;
        protected bool _hitWithRay;
        protected float _rayRunOutTime;
        protected float _hitByRayRefreshTime = 1f;
        protected Color[] _startColours = { Color.red, Color.green, Color.blue, };
        protected IRayReceiver _rayReceiver;
        public bool _addedColour;

        public LineRenderer LaserVisual => _laserVisual;
    }
}
