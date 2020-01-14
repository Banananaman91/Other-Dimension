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
                }
                boid.AddLeader(_rb);
            }
            
            if (_boidSwarm.Count == _flockTotal) StateChange.ToFindTargetState();
        }

        protected override void MoveCharacter()
        {

            Vector3 direction = _goalPosition - _rb.position;
            _rb.MovePosition(_rb.position + direction * movementSpeed * Time.deltaTime);
            if (!(Vector3.Distance(_rb.position, _goalPosition) < 1)) return;
            StateChange.ToFindTargetState();
        }
    }
}
