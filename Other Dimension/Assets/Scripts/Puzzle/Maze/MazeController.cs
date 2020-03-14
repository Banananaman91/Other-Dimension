using System;
using UnityEngine;

namespace Puzzle.Maze
{
    [RequireComponent(typeof(MazeConstructor))]
    public class MazeController : MonoBehaviour
    {
        [SerializeField] private MazeConstructor _generator;
        [SerializeField] private int _rowSize, _columnSize;
        [SerializeField] private float _height, _width;

        private void Start()
        {
            _generator.GenerateNewMaze(_rowSize, _columnSize, _height, _width);
        }
    }
}
