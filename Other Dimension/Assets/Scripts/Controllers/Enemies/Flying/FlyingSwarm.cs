using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers.Enemies.Flying
{
    [RequireComponent(typeof(SphereCollider))]
    public class FlyingSwarm : BaseFlyingAi
    {
        [SerializeField] private GameObject _boid;
        [SerializeField] private int _flockTotal;
        [SerializeField] private int _spawnRadius;
        [SerializeField] private SphereCollider _sphere;

        private BoidRules boidRule = new BoidRules();
        private List<FlyingBoid> _boidSwarm = new List<FlyingBoid>();

        protected override void IdleAction()
        {
            //if (Math.Abs(_sphere.radius - moveableRadius) > float.Epsilon) _sphere.radius = moveableRadius;
            for (int i = 0; i < _flockTotal; i++)
            {
                GameObject clone = Instantiate(_boid);
                clone.transform.position = Random.insideUnitSphere * _spawnRadius;
                _boidSwarm.Add(clone.GetComponent<FlyingBoid>());
            }
            foreach (var boid in _boidSwarm)
            {
                foreach (var t in _boidSwarm.Where(t => !boid.NeighboursRigidbodies.Contains(t.BoidRigidbody) && t != boid))
                {
                    boid.AddNeighbour(t.BoidRigidbody);
                    boid.leader = _rb;
                }
            }
            
            if (_boidSwarm.Count == _flockTotal) StateChange.ToFindTargetState();
        }

        protected override void MoveCharacter()
        {
            for (var i = 0; i < _boidSwarm.Count; i++)
            {
                //var currentBoid = _boidSwarm[i];
                
                _boidSwarm[i].BoidRule1();
                _boidSwarm[i].BoidRule2();
                _boidSwarm[i].BoidRule3();
                _boidSwarm[i].BoidRule4();
                
                // currentBoid.BoidRigidbody.velocity += boidRule.boidRule1(currentBoid.NeighboursRigidbodies);
                // currentBoid.BoidRigidbody.velocity +=
                //     boidRule.boidRule2(currentBoid, currentBoid.NeighboursRigidbodies);
                // currentBoid.BoidRigidbody.velocity += boidRule.boidRule3(currentBoid.NeighboursRigidbodies);

                // _boidSwarm[i] = currentBoid;
                // _boidSwarm[i].transform.position += currentBoid.BoidRigidbody.velocity;
            }
            
            Vector3 direction = _goalPosition - _rb.position;
            _rb.MovePosition(_rb.position + direction * movementSpeed * Time.deltaTime);
            if (!(Vector3.Distance(_rb.position, _goalPosition) < 1)) return;
            StateChange.ToFindTargetState();
            /*
             * for int boids
             * currentBoid = boidint
             * currentBoid.velocity += rule1
             * currentBoid.velocity += rule2
             * currentBoid.velocity += rule3
             * currentBoid.velocity += rule4
             *
             * boidint = currentBoid
             *
             * for int boids
             * boidint position += boidint velocity
             */
        }
    }
}
