using System;
using UnityEngine;

namespace Terrain
{
    public class Planet : MonoBehaviour
    {
        [Range(2, 256)]
        [SerializeField] private int resolution = 10;
        
        [SerializeField, HideInInspector] 
        private MeshFilter[] _meshFilters;
        private TerrainFace[] _terrainFaces;

        private void OnValidate()
        {
            Initialize();
            GenerateMesh();
        }

        void Initialize()
        {
            if (_meshFilters == null || _meshFilters.Length == 0)
            {
                _meshFilters = new MeshFilter[6];
            }

            _terrainFaces = new TerrainFace[6];

            Vector3[] directions =
                {Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back};

            for (int i = 0; i < 6; i++)
            {
                if (_meshFilters[i] == null)
                {
                    GameObject meshObj = new GameObject("mesh");
                    meshObj.transform.parent = transform;

                    meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                    _meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                    _meshFilters[i].sharedMesh = new Mesh();
                }

                _terrainFaces[i] = new TerrainFace(_meshFilters[i].sharedMesh, resolution, directions[i]);
            }
        }

        void GenerateMesh()
        {
            foreach (var terrainFace in _terrainFaces)
            {
                terrainFace.ConstructMesh();
            }
        }
    }
}
