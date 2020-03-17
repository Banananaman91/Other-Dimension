using System;
using Terrain.Colour;
using Terrain.Settings;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Terrain
{
    public class TerrainFace
    {
        private ShapeGenerator _shapeGenerator;
        private Mesh _mesh;
        private int _resolution;
        private Vector3 _localUp;
        private Vector3 _axisA;
        private Vector3 _axisB;
        private float _height = 1;
        private Vector3[] _vertices;
        private int[] _triangles;

        public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
        {
            _shapeGenerator = shapeGenerator;
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
            Vector2[] uv = _mesh.uv;
            
            for (int i = 0; i < _resolution; i++)
            {
                for (int j = 0; j < _resolution; j++)
                {
                    int loop = i + j * _resolution;
                    Vector2 percent = new Vector2(i, j) / (_resolution - 1);
                    Vector3 pointOnUnitCube =
                        _localUp + (percent.x - 0.5f) * 2 * _axisA + (percent.y - 0.5f) * 2 * _axisB;
                    Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                    _vertices[loop] = _shapeGenerator.CalculatePointOnPlanet(pointOnUnitSphere);
                    
                    
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

            _mesh.Clear();
            _mesh.vertices = _vertices;
            _mesh.triangles = _triangles;
            _mesh.RecalculateNormals();
            _mesh.uv = uv;
            InvertNormals();
        }

        public void UpdateUVs(ColourGenerator colourGenerator)
        {
            Vector2[] uv = new Vector2[_resolution * _resolution];

            for (int i = 0; i < _resolution; i++)
            {
                for (int j = 0; j < _resolution; j++)
                {
                    int loop = i + j * _resolution;
                    Vector2 percent = new Vector2(i, j) / (_resolution - 1);
                    Vector3 pointOnUnitCube =
                        _localUp + (percent.x - 0.5f) * 2 * _axisA + (percent.y - 0.5f) * 2 * _axisB;
                    Vector3 pointOnUnitSphere = pointOnUnitCube.normalized;
                    
                    uv[i] = new Vector2(colourGenerator.BiomePercentFromPoint(pointOnUnitSphere), 0);
                }
            }
            _mesh.uv = uv;
        }

        private void InvertNormals()
        {
            Vector3[] normals = _mesh.normals;
            for (int i = 0; i < normals.Length; i++)
                normals[i] = -normals[i];
            _mesh.normals = normals;

            for (int m = 0; m < _mesh.subMeshCount; m++)
            {
                int[] triangles = _mesh.GetTriangles(m);
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    int temp = triangles[i + 0];
                    triangles[i + 0] = triangles[i + 1];
                    triangles[i + 1] = temp;
                }

                _mesh.SetTriangles(triangles, m);
            }

        }
    }
}
