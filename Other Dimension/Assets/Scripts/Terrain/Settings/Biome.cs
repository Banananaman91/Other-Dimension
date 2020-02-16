using System;
using UnityEngine;

namespace Terrain.Settings
{
    [Serializable]
    public class Biome
    {
        [SerializeField] private Gradient _gradient = new Gradient();
        [SerializeField] private Color _tint;
        [Range(0, 1), SerializeField] private float _startHeight;
        [Range(0, 1), SerializeField] private float _tintPercent;

        public Gradient Gradient => _gradient;

        public Color Tint => _tint;

        public float TintPercent => _tintPercent;

        public float StartHeight => _startHeight;
    }
}
