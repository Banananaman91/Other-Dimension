using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Puzzle.Builder
{
    public class BuildingPieces : MonoBehaviour
    {
        [SerializeField] protected WallType _wallType;
        [SerializeField] protected GameObject _objectPoint;
        [SerializeField] protected GameObject[] _puzzleElements;
        private int _elementChoice;
        public Transform ObjectTransform => _objectPoint.transform;
        public WallType WallType => _wallType;

        private void Start()
        {
            var elementChange = Random.Range(0.0f, 1.0f);
            if (!(elementChange >= 0.0f) || !(elementChange <= 0.2f)) return;
            _elementChoice = Random.Range(0, _puzzleElements.Length - 1);
            GameObject go = Instantiate(_puzzleElements[_elementChoice]);
            var position = transform.position;
            go.transform.position = new Vector3(position.x, position.y + 4.5f, position.z);
        }
    }
}
