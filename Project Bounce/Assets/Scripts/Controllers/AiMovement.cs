using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Controllers
{
    public class AiMovement : Controller
    {
        [SerializeField] protected float moveableRadius;
        [SerializeField, Range(1, 10)] protected int stepValue = 1;
        protected float StepValue => stepValue;
        protected Vector3 _goalPosition;
        protected IEnumerable<Vector3> _path = new List<Vector3>();
        public bool PathFinderReady => Pathfinder != null;
        protected Coroutine _gdi;
        protected bool _isMoving;
        protected bool _isFindingPath;
        public AiState _state = AiState.Waiting;
        protected int _element = 0;


        public enum AiState
        {
            Waiting,
            Moving,
            FindingPath,
            FindingTarget
        }

        private void Update()
        {
            switch (_state)
            {
                case AiState.Waiting:
                    if (PathFinderReady) _state = AiState.FindingTarget;
                    break;
                case AiState.Moving:
                    if (_gdi != null && !_isMoving)
                    {
                        StopCoroutine(_gdi);
                        _element = 0;
                    }
                    if (Vector3.Distance(transform.position, _path.ElementAt(_element)) < float.Epsilon)
                    {
                        _element++;
                    }
                    if (_element < _path.Count()) MoveCharacterAcrossPath(_path.ElementAt(_element));
                    break;
                case AiState.FindingPath:
                    if (_gdi != null && !_isFindingPath)
                    {
                        StopCoroutine(_gdi);
                    }
                    if (!_isFindingPath)
                    {
                        _gdi = StartCoroutine(VisualisePath());
                    }
                    break;
                case AiState.FindingTarget:
                    _goalPosition = DetermineGoalPosition();
                    _state = AiState.FindingPath;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(_state), _state, null);
            }
        }

        private IEnumerator VisualisePath()
        {
            _isFindingPath = true;
            yield return StartCoroutine(routine: Pathfinder.FindPath(StepValue, transform.position, _goalPosition, moveableRadius,
                newPath => _path = newPath));
            if (Vector3.Distance(_goalPosition, _path.Last()) < 1)
            {
                _state = AiState.Moving;
                _isFindingPath = false;
            }
        }

        protected virtual Vector3 DetermineGoalPosition()
        {
            var point = Random.insideUnitSphere * moveableRadius;
            return point;
        }

        protected virtual void MoveCharacterAcrossPath(Vector3 location)
        {
            _isMoving = true;

            transform.position = Vector3.MoveTowards(transform.position, location, movementSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _goalPosition) < 1)
            {
                _state = AiState.FindingTarget;
                _isMoving = false;
            }
        }
    }
}