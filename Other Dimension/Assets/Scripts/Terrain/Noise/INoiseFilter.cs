using UnityEngine;

namespace Terrain.Noise
{
    public interface INoiseFilter
    {
        float Evaluate(Vector3 point);
    }
}
