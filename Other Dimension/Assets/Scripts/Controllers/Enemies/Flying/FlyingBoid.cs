using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
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
            _boidRules.BoidRule6(this, _enemyRigidbodies);
        }

        public void AddNeighbour(Rigidbody neighbour)
        {
            _neighboursRigidbodies.Add(neighbour);
        }

        public void AddLeader(Rigidbody leader)
        {
            _leader = leader;
        }

        private void OnTriggerEnter(Collider other)
        {
            var isFlyingBoid = other.GetComponent<FlyingBoid>();
            var rbObject = other.GetComponent<Rigidbody>();
            if (isFlyingBoid || !rbObject) return;
            if (_enemyRigidbodies.Contains(rbObject)) return;
            _enemyRigidbodies.Add(rbObject);
        }

        private void OnTriggerExit(Collider other)
        {
            var isFlyingBoid = other.GetComponent<FlyingBoid>();
            var rbObject = other.GetComponent<Rigidbody>();
            if (isFlyingBoid || !rbObject) return;
            if (!_enemyRigidbodies.Contains(rbObject)) return;
            _enemyRigidbodies.Remove(rbObject);
        }
    }
}
