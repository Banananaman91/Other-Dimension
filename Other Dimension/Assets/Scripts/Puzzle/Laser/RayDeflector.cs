using Puzzle.Laser;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayDeflector : MonoBehaviour, IRayReceiver
{
    public Color LaserColour { get; set; }

    public void HitWithRay()
    {
        throw new System.NotImplementedException();
    }

    public void NotHitWithRay()
    {
        throw new System.NotImplementedException();
    }
}
