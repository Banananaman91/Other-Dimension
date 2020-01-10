using System;
using System.Collections.Generic;
using UnityEngine;

namespace Controllers.Enemies.Flying
{
    [RequireComponent(typeof(Rigidbody), (typeof(SphereCollider)))]
    public class FlyingBoid : MonoBehaviour
    {
        [SerializeField] private int _separationDistance;
        [SerializeField] private SphereCollider _sphere;
        [SerializeField] private int _neighbourRange;
        [SerializeField] private Rigidbody _rb;
        [SerializeField] private float _movementSpeed;
        public Rigidbody leader;

        public int SeparationDistance => _separationDistance;
        public Rigidbody BoidRigidbody => GetComponent<Rigidbody>();
        [SerializeField] private List<Rigidbody> _neighboursRigidbodies = new List<Rigidbody>();
        public List<Rigidbody> NeighboursRigidbodies => _neighboursRigidbodies;


        private void Awake()
        {
            if (Math.Abs(_sphere.radius - _neighbourRange) > float.Epsilon) _sphere.radius = _neighbourRange;
            if (!_sphere.isTrigger) _sphere.isTrigger = true;
        }
        
        public void AddNeighbour(Rigidbody neighbour)
        {
            _neighboursRigidbodies.Add(neighbour);
        }

        public void MoveMe(Vector3 newVelocity)
        {
            _rb.position += newVelocity;
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
        
        public void BoidRule1()
        {
            var average = new Vector3(0, 0, 0);

            if (_neighboursRigidbodies.Count == 0) _rb.velocity = average;

            foreach (var neighbour in _neighboursRigidbodies)
            {
                average += neighbour.position;
            }

            var direction = average / _neighboursRigidbodies.Count;

            _rb.MovePosition(_rb.position + direction * _movementSpeed * Time.deltaTime);
        }

        public void BoidRule2()
        {
            var separation = new Vector3(0, 0, 0);

            foreach (var neighbour in _neighboursRigidbodies)
            {
                var distanceVector = _rb.position - neighbour.position;
                if (Vector3.Distance(_rb.position, neighbour.position) < SeparationDistance) separation -= distanceVector;
            }

            _rb.MovePosition(_rb.position += -separation * _movementSpeed * Time.deltaTime);
        }

        public void BoidRule3()
        {
            var averageVelocity = new Vector3(0, 0, 0);

            foreach (var neighbour in _neighboursRigidbodies)
            {
                averageVelocity += neighbour.velocity;
            }

            _rb.MovePosition(_rb.position += averageVelocity * _movementSpeed * Time.deltaTime);
        }

        public void BoidRule4()
        {
            var direction = leader.position - _rb.position;
            if (Vector3.Distance(_rb.position, leader.position) < _neighbourRange) return;
            _rb.MovePosition(_rb.position + direction * _movementSpeed * Time.deltaTime);
        }
    }
}
