using UnityEngine;

namespace Puzzle.Builder
{
    public class BuildingPieces : MonoBehaviour
    {
        [SerializeField] protected WallType _wallType;
        [SerializeField] protected GameObject _objectPoint;
        [SerializeField] protected GameObject[] _puzzleElements;
        public Transform ObjectTransform => _objectPoint.transform;

        public WallType WallType => _wallType;
    }
}
