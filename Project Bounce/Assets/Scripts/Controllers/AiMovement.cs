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
        [SerializeField] private Vector3 _goalPosition;
        public float moveableRadius;
        private IEnumerable<Vector3> _path = new List<Vector3>();
        private Vector3 _previousLocation, _previousDistance;
        public bool PathFinderReady => Pathfinder != null;
        private Coroutine _gdi;
        private bool _isMoving = true; //we'll clean this up later just getting it working for now
        private bool _isFindingPath = true;


        public enum AiState
        {
            Waiting,
            Moving,
            FindingPath,
            FindingTarget
        }

        private void Start()
        {
            State(AiState.Waiting);
        }

        public void State(AiState state)
        {
            switch (state)
            {
                case AiState.Waiting:
                    if (PathFinderReady) State(AiState.FindingTarget);
                    break;
                case AiState.Moving:
                    if(_gdi != null) StopCoroutine(_gdi);
                    _gdi = StartCoroutine(MoveCharacterAcrossPath());
                    break;
                case AiState.FindingPath:
                    if(_gdi != null) StopCoroutine(_gdi);
                    _gdi = StartCoroutine(VisualisePath());
                    break;
                case AiState.FindingTarget:
                    _goalPosition = DetermineGoalPosition();
                    State(AiState.FindingPath);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(state), state, null);
            }
        }


        private IEnumerator VisualisePath()
        {
            yield return StartCoroutine(routine: Pathfinder.FindPath(transform.position, _goalPosition, moveableRadius,
                newPath => _path = newPath));
            State(AiState.Moving);
        }

        private Vector3 DetermineGoalPosition()
        {
            var point = Random.insideUnitSphere * moveableRadius;
            while (_avoidance.Objects
                .Where(x => Vector3.Distance(x.transform.position, transform.position) < moveableRadius)
                .Any(x => x.RenderBounds.bounds.Contains(point)))
            {
                point = Random.insideUnitSphere * moveableRadius;
            }

            return point;
        }

        private IEnumerator MoveCharacterAcrossPath()
        {
            foreach (var location in _path)
            {
                transform.position = Vector3.MoveTowards(transform.position, location, movementSpeed * Time.deltaTime);
                yield return null; //this has to be here for your deltaTime calculation to work correctly
            }
            State(AiState.FindingTarget);
        }
    }
}