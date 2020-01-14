using System;
using UnityEngine;

namespace Controllers.Enemies.Ground
{
    public class GroundBoid : Boid
    {
        private void Awake()
        {
            if (Math.Abs(_sphere.radius - _neighbourRange) > float.Epsilon) _sphere.radius = _neighbourRange;
            if (!_sphere.isTrigger) _sphere.isTrigger = true;
        }

        private void Update()
        {
            _boidRules.boidRule1(this, _neighboursRigidbodies);
            _boidRules.boidRule2(this, _neighboursRigidbodies);
            _boidRules.boidRule3(this, _neighboursRigidbodies);
            _boidRules.BoidRule4(this);
        }

        public void AddNeighbour(Rigidbody neighbour)
        {
            _neighboursRigidbodies.Add(neighbour);
        }

        public void AddLeader(Rigidbody leader)
        {
            _leader = leader;
        }
    }
}
