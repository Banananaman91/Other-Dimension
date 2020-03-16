using System;
using UnityEngine;

namespace Puzzle.Builder
{
    public class PuzzleStart : MonoBehaviour
    {
        [SerializeField] private PuzzleConstructor _constructor;

        private void Awake()
        {
            _constructor.ConstructPuzzle();
        }
    }
}
