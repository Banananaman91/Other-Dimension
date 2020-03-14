using Controllers;
using UnityEngine;

namespace Puzzle.Laser
{
    public interface IRayInteract
    {
        bool FollowPlayer { get; set; }
        Transform Transform { get; }
        void RayInteraction(PlayerController player);
    }
}
