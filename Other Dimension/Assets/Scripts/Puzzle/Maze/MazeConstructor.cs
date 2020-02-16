using System;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle.Maze
{
    public class MazeConstructor : MonoBehaviour
    {
        [SerializeField] private bool _showDebug;
        [SerializeField] private Material _mazeMaterial1, _mazeMaterial2, _startMaterial, _treasureMaterial;

        private MazeDataGenerator _dataGenerator = new MazeDataGenerator();
        private MazeMeshGenerator _meshGenerator;
        private List<GameObject> _mazeObjects = new List<GameObject>();

        private int[,] Data { get; set; }
        
        private float HallWidth { get; set; }
        private float HallHeight { get; set; }
        private int StartRow { get; set; }
        private int StartColumn { get; set; }
        private int GoalRow { get; set; }
        private int GoalColumn { get; set; }
        
        private void Awake()
        {
            // defaults to walls surrounding a single empty cell
            Data = new int[,]
            {
                {1, 1, 1},
                {1, 0, 1},
                {1, 1, 1}
            };
        }

        public void GenerateNewMaze(int sizeRows, int sizeCols, float height, float width)
        {
            if (sizeRows % 2 == 0 && sizeCols % 2 == 0)
            {
                Debug.LogError("Odd numbers work better");
            }
            
            DisposeOldMaze();

            Data = _dataGenerator.FromDimensions(sizeRows, sizeCols);

            HallWidth = width;
            HallHeight = height;
            _meshGenerator = new MazeMeshGenerator(HallWidth, HallHeight);
            DisplayMaze();

        }

        private void DisplayMaze()
        {
            GameObject go = new GameObject();
            go.transform.position = Vector3.zero;
            go.name = "ProceduralMaze";

            MeshFilter mf = go.AddComponent<MeshFilter>();
            mf.mesh = _meshGenerator.FromData(Data);

            MeshCollider mc = go.AddComponent<MeshCollider>();
            mc.sharedMesh = mf.mesh;

            MeshRenderer mr = go.AddComponent<MeshRenderer>();
            mr.materials = new Material[2] {_mazeMaterial1, _mazeMaterial2};
            _mazeObjects.Add(go);
        }

        private void DisposeOldMaze()
        {
            foreach (var item in _mazeObjects)
            {
                Destroy(item);
            }
        }

        private void FindGoalPosition()
        {
            int[,] maze = Data;
            int rMax = maze.GetUpperBound(0);
            int cMax = maze.GetUpperBound(1);

            for (int i = rMax; i >= 0; i--)
            {
                for (int j = cMax; j >= 0; j--)
                {
                    if (maze[i, j] == 0)
                    {
                        GoalRow = i;
                        GoalColumn = j;
                        return;
                    }
                }
            }
        }

        private void OnGUI()
        {
            if (!_showDebug) return;

            int[,] maze = Data;
            int rMax = maze.GetUpperBound(0);
            int cMax = maze.GetUpperBound(1);

            string msg = "";

            for (int i = rMax; i >= 0; i--)
            {
                for (int j = 0; j <= cMax; j++)
                {
                    if (maze[i, j] == 0) msg += "....";
                    else msg += "==";
                }

                msg += "\n";
            }
            
            GUI.Label(new Rect(20, 20, 500, 500), msg);
        }
    }
}
