using GameMessengerUtilities;
using Interface;
using UnityEngine;

namespace Controllers
{
    [RequireComponent(typeof(Renderer))]
    public class Controller : MonoBehaviour, IObjectAvoidanceInitialisable
    {
        protected IPathfinder Pathfinder;
        private ObjectAvoidance _avoidance;
        private Renderer _renderBounds;
        [SerializeField] protected GameObject pathFinderTiles;
        [SerializeField] protected float movementSpeed;
        [SerializeField] protected float rotationSpeed;
        public Renderer RenderBounds => _renderBounds == null ? _renderBounds : _renderBounds = GetComponent<Renderer>();

        public void Start()
        {
            GetPathfinder();
            AddToObjectAvoidance();
            AvoidMe();
        }

        private void GetPathfinder() => MessageBroker.Instance.SendMessageOfType(new PathFinderRequestMessage(this));

        private void AvoidMe() => _avoidance.Objects.Add(this);
        
        private void AddToObjectAvoidance() => MessageBroker.Instance.SendMessageOfType(new ObjectRequestMessage(this));
        
        public void ObjectInitialise(ObjectAvoidance objectAvoidance) => _avoidance = objectAvoidance;

        public void PathInitialise(IPathfinder pathfinder) => Pathfinder = pathfinder;
    }
}
