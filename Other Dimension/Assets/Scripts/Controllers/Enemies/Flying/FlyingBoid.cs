using System;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers.Enemies.Flying
{
    [RequireComponent(typeof(Rigidbody), (typeof(SphereCollider)))]
    public class FlyingBoid : Boid
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

        public void MoveMe()
        {
            _rb.MovePosition(_rb.position + _direction * _movementSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            var flyingBoid = other.GetComponent<Rigidbody>();
            if (!flyingBoid) return;
            if (_neighboursRigidbodies.Contains(flyingBoid)) return;
            _neighboursRigidbodies.Add(flyingBoid);
        }

        private void OnTriggerExit(Collider other)
        {
            var flyingBoid = other.GetComponent<Rigidbody>();
            if (!flyingBoid) return;
            if (!_neighboursRigidbodies.Contains(flyingBoid)) return;
            _neighboursRigidbodies.Remove(flyingBoid);
        }
    }
}
