using UnityEngine;

namespace Terrain.Noise.NoiseSettings
{
    [System.Serializable]
    public class SimpleNoiseSettings
    {
        [SerializeField] private float _strength = 1;
        [Range(1, 8)]
        [SerializeField] private int _numberOfLayers = 1;

        [SerializeField] private float _baseRoughness = 1;
        [SerializeField] private float _roughness = 2;
        [SerializeField] private float _persistance = 0.5f;
        [SerializeField] private Vector3 _centre;
        [SerializeField] private float _minValue;
        
        public float Strength => _strength;
        public float Roughness => _roughness;
        public Vector3 Centre => _centre;
        public float BaseRoughness => _baseRoughness;
        public int NumberOfLayers => _numberOfLayers;
        public float Persistance => _persistance;
        public float MinValue => _minValue;
    }
}
