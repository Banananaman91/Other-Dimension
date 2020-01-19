using System.Collections.Generic;
using UnityEngine;

namespace Controllers.Enemies
{
    public class Boid : MonoBehaviour
    {
        [SerializeField] protected int _neighbourSeparationDistance;
        [SerializeField] protected int _enemySeparationDistance;
        [SerializeField, Range(0.1f, 1)] protected float _leaderDistance;
        [SerializeField] protected SphereCollider _sphere;
        [SerializeField] protected int _neighbourRange;
        [SerializeField] protected Rigidbody _rb;
        [SerializeField] protected float _movementSpeed;
        [SerializeField] protected List<Rigidbody> _neighboursRigidbodies = new List<Rigidbody>();
        [SerializeField] protected List<Rigidbody> _enemyRigidbodies = new List<Rigidbody>();
        public Rigidbody _leader;
        protected BoidRules _boidRules = new BoidRules();

        public int NeighbourSeparationDistance => _neighbourSeparationDistance;

        public int EnemySeparationDistance => _enemySeparationDistance;

        public float LeaderDistance => _leaderDistance;

        public Rigidbody BoidRigidbody => GetComponent<Rigidbody>();
        
        public List<Rigidbody> NeighboursRigidbodies => _neighboursRigidbodies;

        public float MovementSpeed => _movementSpeed;

        public Rigidbody Leader => _leader;

        public int NeighbourRange => _neighbourRange;
    }
}
