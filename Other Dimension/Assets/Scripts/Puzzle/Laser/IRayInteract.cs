using Controllers;

namespace Puzzle.Laser
{
    public interface IRayInteract
    {
        bool FollowPlayer { get; set; }
        void RayInteraction(PlayerController player);
    }
}
