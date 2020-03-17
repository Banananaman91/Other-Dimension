using Controllers;
using UnityEngine;

namespace GamePhysics
{
    public class GravityController : MonoBehaviour
    {
        [SerializeField] private WhiteHole _gravityCentre;
        [SerializeField] private Controller playerTransform;
        [SerializeField] private int _additive;

        public WhiteHole GravityCentre { get; set; }

        void FixedUpdate()
        {
            if (_gravityCentre && _additive == 0) _gravityCentre.Repel(playerTransform);
            if (_gravityCentre && _additive > 0) _gravityCentre.Repel(playerTransform, _additive);
            if (GravityCentre) GravityCentre.Repel(playerTransform, _additive);
            
        }
    }
}
