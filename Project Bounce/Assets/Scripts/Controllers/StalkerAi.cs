using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Controllers
{
    public class StalkerAi : AiMovement
    {
        [SerializeField] SphereCollider _sphere;

        public List<GameObject> _others = new List<GameObject>();

        protected override void MoveCharacterAcrossPath(Vector3 location)
        {
            _isMoving = true;

            transform.position = Vector3.MoveTowards(transform.position, location, movementSpeed * Time.deltaTime);
            Vector3 direction = transform.position - location;
            transform.Rotate(direction, Space.Self);

            if (Vector3.Distance(transform.position, _goalPosition) < 1)
            {
                _state = AiState.FindingTarget;
                _isMoving = false;
            }
        }

        protected override Vector3 DetermineGoalPosition()
        {
            if (_sphere.radius != moveableRadius) _sphere.radius = moveableRadius;
            Vector3 point;
            if (_others.Count != 0)
            {
                var num = Random.Range(0, _others.Count);
                return point = _others[num].transform.position;
            }
            else
            {
                return point = Random.insideUnitSphere * moveableRadius;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.GetComponent<StalkerAi>())
            {
                if (!_others.Contains(other.gameObject)) _others.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (_others.Contains(other.gameObject)) _others.Remove(other.gameObject);
        }
    }
}
