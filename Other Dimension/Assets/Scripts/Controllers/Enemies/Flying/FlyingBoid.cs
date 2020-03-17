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

        private void FixedUpdate()
        {
            var direction = new Vector3(0, 0, 0);
            direction += _boidRules.boidRule1(this, _neighboursRigidbodies);
            direction += _boidRules.boidRule2(this, _neighboursRigidbodies);
            direction += _boidRules.boidRule3(this, _neighboursRigidbodies);
            direction += _boidRules.BoidRule4(this);
            if (_enemyRigidbodies.Count > 0) direction += _boidRules.BoidRule6(this, _enemyRigidbodies);
            
            BoidRigidbody.velocity = Vector3.ClampMagnitude(BoidRigidbody.velocity, MovementSpeed);
            BoidRigidbody.AddForce(direction.normalized * (MovementSpeed * Time.deltaTime), ForceMode.Impulse);
            //BoidRigidbody.MovePosition(BoidRigidbody.position + transform.forward * (2 * Time.deltaTime));
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
