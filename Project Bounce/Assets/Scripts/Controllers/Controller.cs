using GameMessengerUtilities;
using Interface;
using PathFinding;
using UnityEngine;

namespace Controllers
{
    [RequireComponent(typeof(Renderer))]
    public class Controller : MonoBehaviour, IObjectAvoidanceInitialisable, IObject
    {
        protected IPathfinder Pathfinder;
        protected ObjectAvoidance _avoidance;
        private Renderer _renderBounds;
        [SerializeField] protected float movementSpeed;
        public Renderer RenderBounds => _renderBounds != null ? _renderBounds : _renderBounds = GetComponent<Renderer>();

        public void Awake()
        {
            GetPathfinder();
            AddToObjectAvoidance();
            AvoidMe();
        }

        private void GetPathfinder() => MessageBroker.Instance.SendMessageOfType(new PathFinderRequestMessage(this));

        public void AvoidMe() => _avoidance.Objects.Add(this);
        
        private void AddToObjectAvoidance() => MessageBroker.Instance.SendMessageOfType(new ObjectRequestMessage(this));
        
        public void ObjectInitialise(ObjectAvoidance objectAvoidance) => _avoidance = objectAvoidance;

        public void PathInitialise(IPathfinder pathfinder) => Pathfinder = pathfinder;
    }
}
