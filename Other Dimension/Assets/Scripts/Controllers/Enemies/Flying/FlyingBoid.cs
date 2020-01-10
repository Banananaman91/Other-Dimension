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

        public int SeparationDistance => _separationDistance;
        public Rigidbody BoidRigidbody => GetComponent<Rigidbody>();
        private List<Rigidbody> _neighboursRigidbodies = new List<Rigidbody>();
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
            _neighboursRigidbodies.Add(flyingBoid);
        }

        private void OnTriggerExit(Collider other)
        {
            var flyingBoid = other.GetComponent<Rigidbody>();
            if (!flyingBoid) return;
            _neighboursRigidbodies.Remove(flyingBoid);
        }
        
        public void boidRule1()
        {
            var average = new Vector3(0, 0, 0);

            if (_neighboursRigidbodies.Count == 0) _rb.velocity = average;

            foreach (var neighbour in _neighboursRigidbodies)
            {
                average += neighbour.position;
            }

            _rb.velocity += average / _neighboursRigidbodies.Count;
        }

        public void boidRule2()
        {
            var separation = new Vector3(0, 0, 0);

            foreach (var neighbour in _neighboursRigidbodies)
            {
                var distanceVector = _rb.position - neighbour.position;
                if (Vector3.Distance(_rb.position, neighbour.position) < SeparationDistance) separation -= distanceVector;
            }

            _rb.velocity += -separation;
        }

        public void boidRule3()
        {
            var averageVelocity = new Vector3(0, 0, 0);

            foreach (var neighbour in _neighboursRigidbodies)
            {
                averageVelocity += neighbour.velocity;
            }

            _rb.velocity += averageVelocity;
        }

        // protected override void MoveCharacter()
        // {
        //
        //     for (int i = 0; i < _neighbours.Count; i++)
        //     {
        //         
        //     }
        //     /*
        //      * for int boids
        //      * currentBoid = boidint
        //      * currentBoid.velocity += rule1
        //      * currentBoid.velocity += rule2
        //      * currentBoid.velocity += rule3
        //      * currentBoid.velocity += rule4
        //      *
        //      * boidint = currentBoid
        //      *
        //      * for int boids
        //      * boidint position += boidint velocity
        //      */
        //     
        //     base.MoveCharacter();
        // }
    }
}
