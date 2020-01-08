using Terrain.Noise.NoiseSettings;

namespace Terrain.Noise
{
    public static class NoiseFilterFactory
    {
        public static INoiseFilter CreateNoiseFilter(BaseNoiseSettings settings)
        {
            switch (settings.FilterType)
            {
                case FilterType.Simple:
                    return new SimpleNoiseFilter(settings.SimpleNoiseSettings);
                case FilterType.Ridgid:
                    return new RidgidNoiseFilter(settings.RidgidNoiseSettings);
            }

            return null;
        }
    }
}
