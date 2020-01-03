using UnityEngine;

namespace Terrain.Settings
{
    [CreateAssetMenu()]
    public class ColourSettings : ScriptableObject
    {
        [SerializeField] private BiomeColourSettings _biomeColourSettings;
        [SerializeField] private Material _planetMaterial;

        public Material PlanetMaterial => _planetMaterial;

        public BiomeColourSettings BiomeColourSettings => _biomeColourSettings;
    }
}
