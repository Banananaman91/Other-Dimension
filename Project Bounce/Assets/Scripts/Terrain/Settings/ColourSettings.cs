using UnityEngine;

namespace Terrain.Settings
{
    [CreateAssetMenu()]
    public class ColourSettings : ScriptableObject
    {
        [SerializeField] private Color _planetColour;
        public Color PlanetColour => _planetColour;
    }
}
