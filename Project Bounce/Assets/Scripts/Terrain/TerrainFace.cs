using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Terrain
{
    public class TerrainFace
    {
        private Mesh _mesh;
        private int _resolution;
        private Vector3 _localUp;
        private Vector3 _axisA;
        private Vector3 _axisB;
        private float _height = 1;
        private Vector3[] _vertices;
        private int[] _triangles;

        public TerrainFace(Mesh mesh, int resolution, Vector3 localUp)
        {
            _mesh = mesh;
            _resolution = resolution;
            _localUp = localUp;
            
            _axisA = new Vector3(localUp.y, localUp.z, localUp.x);
            _axisB = Vector3.Cross(localUp, _axisA);
        }

        public void ConstructMesh()
        {
            _vertices = new Vector3[_resolution * _resolution];
            _triangles = new int[(_resolution - 1) * (_resolution - 1) * 6];
            int triIndex = 0;
            for (int i = 0; i < _resolution; i++)
            {
                for (int j = 0; j < _resolution; j++)
                {
                    int loop = i + j * _resolution;
                    Vector2 percent = new Vector2(i, j) / (_resolution - 1);
                    Vector3 pointOnUnitCube =
                        _localUp + (percent.x - 0.5f) * 2 * _axisA + (percent.y - 0.5f) * 2 * _axisB;
                    Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                    _vertices[loop] = pointOnUnitSphere;
                    
                    
                    if (i != _resolution - 1 && j != _resolution - 1)
                    {
                        _triangles[triIndex] = loop;
                        _triangles[triIndex + 1] = loop + _resolution + 1;
                        _triangles[triIndex + 2] = loop + _resolution;
                        
                        _triangles[triIndex + 3] = loop;
                        _triangles[triIndex + 4] = loop + 1;
                        _triangles[triIndex + 5] = loop + _resolution + 1;
                        triIndex += 6;
                    }
                }
            }
            
            int iterations = (int) Mathf.Log(_resolution, 2);
            int numberOfSquares = 1;
            int squareSize = _resolution;
            for (int i = 0; i < iterations; i++)
            {
                int row = 0;
                for (int j = 0; j < numberOfSquares; j++)
                {
                    int collumn = 0;
                    for (int k = 0; k < numberOfSquares; k++)
                    {
                        DiamondSquare(row, collumn, squareSize, _height);
                        collumn += squareSize;
                    }

                    row += squareSize;
                }

                numberOfSquares *= 2;
                squareSize /= 2;
                _height *= 0.5f;
            }
            
            
            _mesh.Clear();
            _mesh.vertices = _vertices;
            _mesh.triangles = _triangles;
            _mesh.RecalculateNormals();
        }
        
        void DiamondSquare(int row, int col, int size, float offset)
        {
            int halfSize = (int) (size * 0.5f);
            int topLeft = row * (_resolution + 1) + col;
            int bottomLeft = (row + size) * (_resolution + 1) + col;

            int mid = (row + halfSize) * (_resolution + 1) + (col + halfSize);
            _vertices[mid].y = (_vertices[topLeft].y + _vertices[topLeft + size].y + _vertices[bottomLeft].y +
                             _vertices[bottomLeft + size].y)*0.25f + Random.Range(-offset, offset);

            _vertices[topLeft + halfSize].y = (_vertices[topLeft].y + _vertices[topLeft + size].y + _vertices[mid].y) / 3 + Random.Range(-offset, offset);
            _vertices[mid - halfSize].y = (_vertices[topLeft].y + _vertices[bottomLeft].y + _vertices[mid].y) / 3 + Random.Range(-offset, offset);
            _vertices[mid + halfSize].y = (_vertices[topLeft + size].y + _vertices[bottomLeft + size].y + _vertices[mid].y) / 3 +
                                       Random.Range(-offset, offset);
            _vertices[bottomLeft + halfSize].y = (_vertices[bottomLeft].y + _vertices[bottomLeft + size].y + _vertices[mid].y) / 3 +
                                              Random.Range(-offset, offset);
        }
    }
}
