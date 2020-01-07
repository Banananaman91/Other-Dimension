﻿using System;
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
        private Coroutine _pathCoroutine;
        private bool _isMoving;
        private bool _isFindingPath;
        public AiState _state = AiState.Waiting;
        private int _element = 0;


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
                    MoveCharacterAcrossPath(_path.ElementAt(_element));
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
            yield return _pathCoroutine = StartCoroutine(routine: Pathfinder.FindPath(transform.position, _goalPosition, moveableRadius,
                newPath => _path = newPath));
            if (Vector3.Distance(_goalPosition, _path.Last()) < 1)
            {
                _state = AiState.Moving;
                _isFindingPath = false;
            }
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

        private void MoveCharacterAcrossPath(Vector3 location)
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