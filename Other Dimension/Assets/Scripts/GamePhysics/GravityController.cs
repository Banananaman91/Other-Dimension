using Controllers;
using UnityEngine;

namespace GamePhysics
{
    public class GravityController : MonoBehaviour
    {
        [SerializeField] private WhiteHole _gravityCentre;
        [SerializeField] private Controller playerTransform;

        void FixedUpdate()
        {
            if (_gravityCentre)
            {
                _gravityCentre.Repel(playerTransform);
            }
        }
    }
}
