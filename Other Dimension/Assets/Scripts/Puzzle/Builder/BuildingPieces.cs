using System;
using Controllers;
using GamePhysics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Puzzle.Builder
{
    public class BuildingPieces : Controller
    {
        [SerializeField] protected WallType _wallType;
        [SerializeField] protected GameObject _objectPoint;
        [SerializeField] protected GameObject[] _puzzleElements;

        private int _elementChoice;
        public Transform ObjectTransform => _objectPoint.transform;
        public WallType WallType => _wallType;
    }
}
