using GameMessengerUtilities;
using Interface;
using PathFinding;
using UnityEngine;

namespace Terrain
{
    [RequireComponent(typeof(Renderer))]
    public class AreaRestriction : MonoBehaviour, IObject, IObjectAvoidanceInitialisable
    {
        protected ObjectAvoidance _avoidance;
        private Renderer _renderBounds;
        public Renderer RenderBounds => _renderBounds != null ? _renderBounds : _renderBounds = GetComponent<Renderer>();
        
        public void Start()
        {
            AddToObjectAvoidance();
            AvoidMe();
        }
        
        public void AvoidMe() => _avoidance.Container = this;
        
        private void AddToObjectAvoidance() => MessageBroker.Instance.SendMessageOfType(new ObjectRequestMessage(this));
        
        public void ObjectInitialise(ObjectAvoidance objectAvoidance) => _avoidance = objectAvoidance;
    }
}
