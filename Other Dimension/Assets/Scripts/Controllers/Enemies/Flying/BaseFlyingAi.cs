using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers.Enemies.Flying
{
    public class BaseFlyingAi : AiMaster
    {
        [SerializeField] protected float moveableRadius;
        [SerializeField, Range(1, 10)] protected int stepValue = 1;
        [SerializeField, Range(2, 10)] protected int attackValue = 2;
        [SerializeField] protected int attackDistance;
        [SerializeField] protected bool canAttack;
        [SerializeField] protected Rigidbody _rb;
        protected float StepValue => stepValue;
        protected IEnumerable<Vector3> _path = new List<Vector3>();
        protected bool PathFinderReady => Pathfinder != null;
        protected bool _isMoving;
        protected int _element;

        protected GameObject _target;
        // public override void Update()
        // {
        //     switch (State)
        //     {
        //         case AiState.Idle:
        //             if (PathFinderReady) StateChange.ToFindTargetState();  //_state = AiState.FindingTarget;
        //             break;
        //         case AiState.Moving:
        //             if (_gdi != null && !_isMoving)
        //             {
        //                 StopCoroutine(_gdi);
        //                 _element = 0;
        //             }
        //             if (Vector3.Distance(transform.position, _path.ElementAt(_element)) < float.Epsilon)
        //             {
        //                 _element++;
        //             }
        //             if (_element < _path.Count()) MoveCharacterAcrossPath(_path.ElementAt(_element));
        //             break;
        //         case AiState.FindingPath:
        //             if (_gdi != null && !_isFindingPath)
        //             {
        //                 StopCoroutine(_gdi);
        //             }
        //             if (!_isFindingPath)
        //             {
        //                 _gdi = StartCoroutine(VisualisePath());
        //             }
        //             break;
        //         case AiState.FindingTarget:
        //             _goalPosition = DetermineGoalPosition();
        //             StateChange.ToFindPathState();  //_state = AiState.FindingPath;
        //             break;
        //         case AiState.Attack:
        //         case AiState.Capture:
        //         case AiState.Chase:
        //         case AiState.Block:
        //         case AiState.Alert:
        //         case AiState.Maneuver:
        //         case AiState.Strike:
        //             break;
        //         default:
        //             throw new ArgumentOutOfRangeException(nameof(State), State, null);
        //     }
        // }
        protected override void IdleAction()
        {
            if (!PathFinderReady) return;
            StateChange.ToFindTargetState();
        }

        protected override IEnumerator VisualisePath()
        {
            _isFindingPath = true;
            yield return StartCoroutine(routine: Pathfinder.FindPath(StepValue, transform.position, _goalPosition,
                moveableRadius,
                newPath => _path = newPath));
            if (!(Vector3.Distance(_goalPosition, _path.Last()) < 1)) yield break;
            _usingPath = true;
            StateChange.ToMoveState(); //_state = AiState.Moving;
            _isFindingPath = false;
        }

        protected override void DetermineGoalPosition()
        {
            _goalPosition = Random.insideUnitSphere * moveableRadius;
            StateChange.ToMoveState();
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

                if (_rb.isKinematic)
                {
                    Vector3 direction = _goalPosition - _rb.position;
                    _rb.MovePosition(_rb.position + direction * movementSpeed * Time.deltaTime);
                    //transform.Rotate(direction, Space.Self);
                }


                if (!(Vector3.Distance(_rb.position, _goalPosition) < 1)) return;
                StateChange.ToFindTargetState();
            }
        }

        private void MoveAcrossPath(Vector3 location)
        {
            _isMoving = true;

            transform.position =
                Vector3.MoveTowards(transform.position, location, movementSpeed * Time.deltaTime);
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
                _rb.AddForce(direction * (movementSpeed * attackValue), ForceMode.Acceleration);
            }
            _timer = _attackCooldownTime;
            _attackCooldown = true;
            StateChange.ToFindTargetState();
        }
    }
}