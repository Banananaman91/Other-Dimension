using System;
using Terrain.Noise.NoiseSettings;
using UnityEngine;

namespace Terrain.Settings
{
    [Serializable]
    public class BiomeColourSettings
    {
        [SerializeField] private Biome[] _biomes;
        [SerializeField] private BaseNoiseSettings _noiseSettings;
        [Range(0, 1), SerializeField] private float _blendAmount;
        private float _noiseOffset;
        private float _noiseStrength;

        public Biome[] Biomes => _biomes;

        public BaseNoiseSettings NoiseSettings => _noiseSettings;

        public float NoiseOffset => _noiseOffset;

        public float NoiseStrength => _noiseStrength;

        public float BlendAmount => _blendAmount;
    }
}
