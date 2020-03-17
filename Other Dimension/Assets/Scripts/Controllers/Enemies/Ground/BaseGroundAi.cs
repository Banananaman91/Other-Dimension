using System.Collections;
using System.Collections.Generic;
using System.Linq;
using PathFinding;
using Puzzle.Builder;
using UnityEngine;

namespace Controllers.Enemies.Ground
{
    [RequireComponent(typeof(Rigidbody))]
    public class BaseGroundAi : AiMaster
    {
        [Header("Base Ground")] [SerializeField]
        protected int movementSpeed;
        [SerializeField, Range(1, 30)] protected int moveableRadius;
        [SerializeField, Range(1, 10)] protected int stepValue = 1;
        [SerializeField, Range(2, 10)] protected int attackValue = 2;
        [SerializeField] protected int attackDistance;
        [SerializeField] protected bool canAttack;
        [SerializeField] protected PuzzleConstructor _goal;
        protected float StepValue => stepValue;
        protected IEnumerable<Vector3> _path = new List<Vector3>();
        protected PathFinder Pathfinder = new PathFinder();
        protected bool PathFinderReady => Pathfinder != null;
        protected bool _isMoving;
        protected int _element;

        protected GameObject _target;

        protected override void IdleAction()
        {
            if (!PathFinderReady) return;
            StateChange.ToFindTargetState();
        }

        protected override IEnumerator VisualisePath()
        {
            _isFindingPath = true;
            yield return StartCoroutine(Pathfinder.FindPath(StepValue, transform.position, _goalPosition,
                moveableRadius,
                newPath => _path = newPath));
            if (!(Vector3.Distance(_goalPosition, _path.Last()) < 1)) yield break;
            _usingPath = true;
            StateChange.ToMoveState();
            _isFindingPath = false;
        }

        protected override void DetermineGoalPosition()
        {
            var localPosition = transform.localPosition;
            _goalPosition = _goal ? _goal.Goal.transform.position : new Vector3(Random.Range(localPosition.x - moveableRadius, localPosition.x + moveableRadius), localPosition.y, Random.Range(localPosition.z - moveableRadius, localPosition.z + moveableRadius));
            Debug.Log(_goalPosition);
            if (!_goal) StateChange.ToMoveState();
            else StateChange.ToFindPathState();
        }

        protected override void MoveCharacter()
        {
            if (_usingPath)
            {
                if (_gdi != null && !_isMoving)
                {
                    StopCoroutine(_gdi);
                    _element = 0;
                }

                if (Vector3.Distance(transform.position, _path.ElementAt(_element)) < float.Epsilon)
                {
                    _element++;
                }

                if (_element < _path.Count()) MoveAcrossPath(_path.ElementAt(_element));
            }
            else
            {
                if (canAttack && _target && !_attackCooldown)
                {
                    if (Vector3.Distance(_rb.position, _target.transform.position) < attackDistance)
                    {
                        StateChange.ToAttackState();
                    }
                }

                if (_attackCooldown && _rb.isKinematic == false)
                {
                    if (_timer <= _attackCooldownTime - 2) _rb.isKinematic = true;
                }

                if (!_rb.isKinematic)
                {
                    Vector3 direction = _goalPosition - _rb.position;
                    //BoidRigidbody.velocity = Vector3.ClampMagnitude(BoidRigidbody.velocity, MovementSpeed);
                    _rb.AddForce(direction.normalized * (movementSpeed * Time.deltaTime), ForceMode.VelocityChange);
                    //transform.Rotate(direction, Space.Self);
                }


                if (!(Vector3.Distance(_rb.position, _goalPosition) < 1)) return;
                StateChange.ToFindTargetState();
            }
        }

        private void MoveAcrossPath(Vector3 location)
        {
            _isMoving = true;
            var direction = location - _rb.position;
            _rb.AddForce(direction.normalized * (movementSpeed * Time.deltaTime), ForceMode.VelocityChange);
            //transform.position = Vector3.MoveTowards(transform.position, location, movementSpeed * Time.deltaTime);
            if (!(Vector3.Distance(transform.position, _goalPosition) < 1)) return;
            _usingPath = false;
            StateChange.ToFindTargetState();
            _isMoving = false;
        }

        protected override void Attack()
        {
            _rb.isKinematic = false;
            if (_rb)
            {
                Vector3 direction = _target.transform.position - _rb.position;
                _rb.AddForce(direction * (movementSpeed * attackValue), ForceMode.Impulse);
            }
            _timer = _attackCooldownTime;
            _attackCooldown = true;
            StateChange.ToFindTargetState();
        }

        protected override void Block()
        {
            throw new System.NotImplementedException();
        }
    }
}
