using UnityEngine;

namespace Terrain.Noise.NoiseSettings
{
    [System.Serializable]
    public class RidgidNoiseSettings : SimpleNoiseSettings
    {
        [SerializeField] private float _weightMultiplier = 0.8f;
        public float WeightMultiplier => _weightMultiplier;
    }
}
