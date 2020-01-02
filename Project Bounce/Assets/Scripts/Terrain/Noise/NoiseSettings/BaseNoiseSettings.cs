using UnityEngine;

namespace Terrain.Noise.NoiseSettings
{
    [System.Serializable]
    public class BaseNoiseSettings
    {
        
        [SerializeField] private FilterType _filterType;
        [ConditionalHide("_filterType", 0)]
        [SerializeField] private SimpleNoiseSettings _simpleNoiseSettings;
        [ConditionalHide("_filterType", 1)]
        [SerializeField] private RidgidNoiseSettings _ridgidNoiseSettings;
        public FilterType FilterType => _filterType;

        public SimpleNoiseSettings SimpleNoiseSettings => _simpleNoiseSettings;

        public RidgidNoiseSettings RidgidNoiseSettings => _ridgidNoiseSettings;
    }
}
