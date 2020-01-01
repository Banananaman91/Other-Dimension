using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Terrain
{
    public class DiamondSquareTerrain : MonoBehaviour
    {
        [SerializeField] private int _divisions;
        [SerializeField] private float _size;
        [SerializeField] private float _height;
        

        private Vector3[] _verts;
        private int _vertCount;

        private void Start()
        {
            CreateTerrain();
        }

        void CreateTerrain()
        {
            _vertCount = (_divisions + 1) * (_divisions + 1);
            _verts = new Vector3[_vertCount];
            Vector2[] uvs = new Vector2[_vertCount];
            int[] tris = new int[_divisions*_divisions*6];

            float halfSize = _size * 0.5f;
            float divisionSize = _size / _divisions;
            
            Mesh mesh = new Mesh();
            GetComponent<MeshFilter>().mesh = mesh;

            int triOffset = 0;

            for (int i = 0; i <= _divisions; i++)
            {
                for (int j = 0; j <= _divisions; j++)
                {
                    _verts[i*(_divisions+1)+j] = new Vector3(-halfSize+j*divisionSize, 0.0f, halfSize-i*divisionSize);
                    uvs[i*(_divisions+1)+j] = new Vector2((float)i/_divisions, (float)j/_divisions);

                    if (i < _divisions && j < _divisions)
                    {
                        int topLeft = i * (_divisions + 1) + j;
                        int bottomLeft = (i + 1) * (_divisions + 1) + j;

                        tris[triOffset] = topLeft;
                        tris[triOffset + 1] = topLeft + 1;
                        tris[triOffset + 2] = bottomLeft + 1;

                        tris[triOffset + 3] = topLeft;
                        tris[triOffset + 4] = bottomLeft + 1;
                        tris[triOffset + 5] = bottomLeft;

                        triOffset += 6;
                    }
                }
            }

            _verts[0].y = Random.Range(-_height, _height);
            _verts[_divisions].y = Random.Range(-_height, _height);
            _verts[_verts.Length - 1].y = Random.Range(-_height, _height);
            _verts[_verts.Length - 1 - _divisions].y = Random.Range(-_height, _height);

            int iterations = (int) Mathf.Log(_divisions, 2);
            int numberOfSquares = 1;
            int squareSize = _divisions;
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

            mesh.vertices = _verts;
            mesh.uv = uvs;
            mesh.triangles = tris;
            mesh.RecalculateBounds();
            mesh.RecalculateNormals();
        }

        void DiamondSquare(int row, int col, int size, float offset)
        {
            int halfSize = (int) (size * 0.5f);
            int topLeft = row * (_divisions + 1) + col;
            int bottomLeft = (row + size) * (_divisions + 1) + col;

            int mid = (int) (row + halfSize) * (_divisions + 1) + (int) (col + halfSize);
            _verts[mid].y = (_verts[topLeft].y + _verts[topLeft + size].y + _verts[bottomLeft].y +
                          _verts[bottomLeft + size].y)*0.25f + Random.Range(-offset, offset);

            _verts[topLeft + halfSize].y = (_verts[topLeft].y + _verts[topLeft + size].y + _verts[mid].y) / 3 + Random.Range(-offset, offset);
            _verts[mid - halfSize].y = (_verts[topLeft].y + _verts[bottomLeft].y + _verts[mid].y) / 3 + Random.Range(-offset, offset);
            _verts[mid + halfSize].y = (_verts[topLeft + size].y + _verts[bottomLeft + size].y + _verts[mid].y) / 3 +
                                       Random.Range(-offset, offset);
            _verts[bottomLeft + halfSize].y = (_verts[bottomLeft].y + _verts[bottomLeft + size].y + _verts[mid].y) / 3 +
                                              Random.Range(-offset, offset);
        }
    }
}
