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
        private AiState state = AiState.waiting;


        public enum AiState
        {
            waiting,
            moving,
            findingPath,
            findingTarget
        }

        public void Update()
        {
            switch (state)
            {
                case AiState.waiting:
                    if (PathFinderReady) state = AiState.findingTarget;
                    break;
                case AiState.moving:
                    StartCoroutine(MoveCharacterAcrossPath());
                    state = AiState.findingTarget;
                    break;
                case AiState.findingPath:
                    StartCoroutine(VisualisePath());
                    state = AiState.moving;
                    break;
                case AiState.findingTarget:
                    _goalPosition = DetermineGoalPosition();
                    state = AiState.findingPath;
                    break;
            }
        }
        
        
        private IEnumerator VisualisePath()
        {
            yield return StartCoroutine(routine: Pathfinder.FindPath(transform.position, _goalPosition, moveableRadius, newPath => _path = newPath));
            yield return null;
        }
        
        private Vector3 DetermineGoalPosition()
        {
            var point = Random.insideUnitSphere * moveableRadius;
            while (_avoidance.Objects.Any(x => x.RenderBounds.bounds.Contains(point)))
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
            }
            yield return null;
        }
    }
}
