using System;
using Terrain.Colour;
using Terrain.Settings;
using UnityEngine;

namespace Terrain
{
    public class Planet : MonoBehaviour
    {
        [Range(2, 256)]
        [SerializeField] private int resolution = 10;

        [SerializeField] private ShapeSettings _shapeSettings;
        [SerializeField] private ColourSettings _colourSettings;
        [SerializeField] private FaceRenderMask _faceRenderMask;
        
        [SerializeField, HideInInspector] 
        private MeshFilter[] _meshFilters;
        private TerrainFace[] _terrainFaces;
        private ShapeGenerator _shapeGenerator = new ShapeGenerator();
        private ColourGenerator _colourGenerator = new ColourGenerator();

        public ShapeSettings ShapeSettings => _shapeSettings;
        public ColourSettings ColourSettings => _colourSettings;

        [HideInInspector] 
        public bool shapeSettingsFoldout;
        [HideInInspector]
        public bool colourSettingsFoldout;

        private void Awake()
        {
            Initialize();
            GenerateColours();
            GenerateMesh();
        }

        void Initialize()
        {
            _shapeGenerator.UpdateSettings(_shapeSettings);
            _colourGenerator.UpdateSettings(_colourSettings);
            
            if (_meshFilters == null || _meshFilters.Length == 0)
            {
                _meshFilters = new MeshFilter[6];
                foreach (var mesh in _meshFilters)
                {
                    mesh.gameObject.AddComponent<MeshCollider>();
                }
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

                    meshObj.AddComponent<MeshRenderer>();
                    _meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                    _meshFilters[i].sharedMesh = new Mesh();
                }

                _meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = _colourSettings.PlanetMaterial;

                _terrainFaces[i] = new TerrainFace(_shapeGenerator, _meshFilters[i].sharedMesh, resolution, directions[i]);
                bool renderFace = _faceRenderMask == FaceRenderMask.All || (int) _faceRenderMask - 1 == i;
                _meshFilters[i].gameObject.SetActive(renderFace);
            }
        }

        public void GeneratePlanet()
        {
            Initialize();
            GenerateMesh();
            GenerateColours();
        }
        
        public void OnShapeSettingsUpdated()
        {
            Initialize();
            GenerateMesh();
        }

        public void OnColourSettingsUpdated()
        {
            Initialize();
            GenerateColours();
        }

        void GenerateMesh()
        {
            for (int i = 0; i < 6; i++)
            {
                if (_meshFilters[i].gameObject.activeSelf)
                {
                    _terrainFaces[i].ConstructMesh();
                }
            }
            
            _colourGenerator.UpdateElevation(_shapeGenerator.ElevationMinMax);
        }

        void GenerateColours()
        {
            _colourGenerator.UpdateColours();
            for (int i = 0; i < 6; i++)
            {
                if (_meshFilters[i].gameObject.activeSelf)
                {
                    _terrainFaces[i].UpdateUVs(_colourGenerator);
                }
            }
            
        }
    }
}
