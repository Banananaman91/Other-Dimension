using System;
using UnityEngine;

namespace Puzzle.Laser
{
    public class RayDestructable : MonoBehaviour, IRayReceiver
    {
        private float _t;
        private int _speed = 10;
        public Color LaserColour { get; set; }
        private Color TargetColour = Color.yellow;
        private Material WallMaterial => GetComponent<MeshRenderer>().material;
        private Color CurrentColour => WallMaterial.GetColor(BaseColor);
        private Color WallColour;
        private static readonly int BaseColor = Shader.PropertyToID("_BaseColor");
        private Ray _ray;

        private void Awake()
        {
            WallMaterial.SetColor(BaseColor, Color.cyan);
            WallColour = WallMaterial.GetColor(BaseColor);
        }

        public void HitWithRay(Ray ray = null)
        {
            _ray = ray;
            WallMaterial.SetColor(BaseColor, Color.Lerp(WallColour, TargetColour, _t));
            if (CurrentColour == TargetColour)
            {
                if(_ray) ray._addedColour = false;
                Destroy(gameObject);
            }
            if (_t < 1) _t += Time.deltaTime / _speed;
        }

        public void NotHitWithRay()
        {
            WallMaterial.color = Color.Lerp(TargetColour, WallColour, _t);
            if (_t > 0) _t -= Time.deltaTime / _speed;
        }
    }
}
