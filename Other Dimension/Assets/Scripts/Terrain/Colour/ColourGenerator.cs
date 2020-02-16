using Terrain.Noise;
using Terrain.Settings;
using UnityEngine;

namespace Terrain.Colour
{
    public class ColourGenerator
    {
        private ColourSettings _settings;
        private Texture2D _texture2D;
        private const int _textureResolution = 50;
        private INoiseFilter biomeNoiseFilter;

        public void UpdateSettings(ColourSettings settings)
        {
            _settings = settings;
            if (_texture2D == null || _texture2D.height != settings.BiomeColourSettings.Biomes.Length)
            {
                _texture2D = new Texture2D(_textureResolution, settings.BiomeColourSettings.Biomes.Length);
            }
            biomeNoiseFilter = NoiseFilterFactory.CreateNoiseFilter(settings.BiomeColourSettings.NoiseSettings);
        }

        public void UpdateElevation(MinMax elevationMinMax)
        {
            _settings.PlanetMaterial.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.MaX));
        }

        public float BiomePercentFromPoint(Vector3 pointOnUnitSphere)
        {
            float heightPercent = (pointOnUnitSphere.y + 1) / 2f;
            heightPercent += (biomeNoiseFilter.Evaluate(pointOnUnitSphere) - _settings.BiomeColourSettings.NoiseOffset) * _settings.BiomeColourSettings.NoiseStrength;
            float biomeIndex = 0;
            int numBiomes = _settings.BiomeColourSettings.Biomes.Length;
            float blendRange = _settings.BiomeColourSettings.BlendAmount / 2f + 0.001f;

            for (int i = 0; i < numBiomes; i++)
            {
                float distance = heightPercent - _settings.BiomeColourSettings.Biomes[i].StartHeight;
                float weight = Mathf.InverseLerp(-blendRange, blendRange, distance);
                biomeIndex *= 1 - weight;
                biomeIndex += i * weight;
            }

            return biomeIndex / Mathf.Max(1, numBiomes - 1);
        }

        public void UpdateColours()
        {
            Color[] colours = new Color[_texture2D.width * _texture2D.height];
            int colourIndex = 0;

            foreach (var biome in _settings.BiomeColourSettings.Biomes)
            {
                for (int i = 0; i < _textureResolution; i++)
                {
                    Color gradientColour = biome.Gradient.Evaluate(i / (_textureResolution - 1f));
                    Color tintColour = biome.Tint;
                    colours[colourIndex] = gradientColour * (1 - biome.TintPercent) + tintColour * biome.TintPercent;
                    colourIndex++;
                }
            }

            _texture2D.SetPixels(colours);
            _texture2D.Apply();
            _settings.PlanetMaterial.SetTexture("_texture", _texture2D);
        }
    }
}
