using System.Collections.Generic;
using UnityEngine;

namespace Puzzle.Laser
{
    public interface IRayReceiver
    {
        Color LaserColour { get; set; }
        void HitWithRay(Ray ray = null);
        void NotHitWithRay();
    }
}
