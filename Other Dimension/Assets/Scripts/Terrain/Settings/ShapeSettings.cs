using Terrain.Noise;
using Terrain.Noise.NoiseSettings;
using UnityEngine;

namespace Terrain.Settings
{
    [CreateAssetMenu()]
    public class ShapeSettings : ScriptableObject
    {
        [SerializeField] private float _planetRadius;
        [SerializeField] private NoiseLayer[] _noiseLayers;
        public float PlanetRadius => _planetRadius;
        public NoiseLayer[] NoiseLayers => _noiseLayers;

        [System.Serializable]
        public class NoiseLayer
        {
            [SerializeField] private bool enabled = true;
            [SerializeField] private bool useFirstLayerMask;
            [SerializeField] private BaseNoiseSettings baseNoiseSettings;

            public bool Enabled => enabled;

            public bool UseFirstLayerMask => useFirstLayerMask;

            public BaseNoiseSettings BaseNoiseSettings => baseNoiseSettings;
        }
    }
}
