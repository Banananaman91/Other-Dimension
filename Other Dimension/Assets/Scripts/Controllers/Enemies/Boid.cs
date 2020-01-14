using System.Collections.Generic;
using UnityEngine;

namespace Controllers.Enemies
{
    public class Boid : MonoBehaviour
    {
        [SerializeField] protected int _separationDistance;
        [SerializeField] protected SphereCollider _sphere;
        [SerializeField] protected int _neighbourRange;
        [SerializeField] protected Rigidbody _rb;
        [SerializeField] protected float _movementSpeed;
        [SerializeField] protected List<Rigidbody> _neighboursRigidbodies = new List<Rigidbody>();
        protected Rigidbody _leader;
        protected Vector3 _direction;
        protected BoidRules _boidRules = new BoidRules();

        public int SeparationDistance => _separationDistance;
        public Rigidbody BoidRigidbody => GetComponent<Rigidbody>();
        
        public List<Rigidbody> NeighboursRigidbodies => _neighboursRigidbodies;

        public float MovementSpeed => _movementSpeed;

        public Rigidbody Leader => _leader;

        public int NeighbourRange => _neighbourRange;
    }
}
