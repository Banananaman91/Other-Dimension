using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers.Enemies.Flying
{
    public class StalkerAi : BaseFlyingAi
    {
        [Header("Stalker")]
        [SerializeField] SphereCollider _sphere;
        [SerializeField] int _stalkDistance = 1;

        public List<GameObject> _others = new List<GameObject>();

        protected override void DetermineGoalPosition()
        {
            if (Math.Abs(_sphere.radius - moveableRadius) > float.Epsilon) _sphere.radius = moveableRadius;
            if (_others.Count == 0)
            {
                _goalPosition = Random.insideUnitSphere * moveableRadius;
            }
            else
            {
                var num = Random.Range(0, _others.Count);
                _goalPosition = _others[num].transform.position - _others[num].transform.forward * _stalkDistance;
                _target = _others[num];
            }
            StateChange.ToMoveState();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<StalkerAi>()) return;
            if (!other.GetComponent<BaseFlyingAi>() && !other.GetComponent<FlyingBoid>()) return;
            if (!_others.Contains(other.gameObject)) _others.Add(other.gameObject);
        }

        private void OnTriggerExit(Collider other)
        {
            if (_others.Contains(other.gameObject)) _others.Remove(other.gameObject);
        }
    }
}
