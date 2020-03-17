using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GamePhysics;
using UnityEngine;

namespace Controllers.Enemies.Ground
{
    public class GroundSwarm : BaseGroundAi
    {
        [Header("Swarm")]
        [SerializeField] private GameObject _boid;
        [SerializeField, Range(1, 50)] private int _flockTotal;
        [SerializeField] private int _spawnRadius;
        [SerializeField] private WhiteHole _whiteHole;
        
        private List<GroundBoid> _boidSwarm = new List<GroundBoid>();
        protected override void IdleAction()
        {
            //if (Math.Abs(_sphere.radius - moveableRadius) > float.Epsilon) _sphere.radius = moveableRadius;
            for (int i = 0; i < _flockTotal; i++)
            {
                GameObject clone = Instantiate(_boid);
                var transformPosition = transform.position;
                clone.transform.position = new Vector3(Random.Range(transformPosition.x - _spawnRadius, transformPosition.x + _spawnRadius), transformPosition.y, Random.Range(transformPosition.z - _spawnRadius, transformPosition.z + _spawnRadius));
                _boidSwarm.Add(clone.GetComponent<GroundBoid>());
                var gravityComponent = clone.GetComponent<GravityController>();
                if (gravityComponent) gravityComponent.GravityCentre = _whiteHole;
            }
            foreach (var boid in _boidSwarm)
            {
                foreach (var t in _boidSwarm.Where(t => !boid.NeighboursRigidbodies.Contains(t.BoidRigidbody) && t != boid))
                {
                    boid.AddNeighbour(t.BoidRigidbody);
                    
                }
                boid.AddLeader(_rb);
                boid.AddNeighbour(_rb);
            }
            
            if (_boidSwarm.Count == _flockTotal) StateChange.ToFindTargetState();
        }
    }
}
