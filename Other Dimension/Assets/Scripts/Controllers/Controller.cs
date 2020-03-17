using GameMessengerUtilities;
using GameOctree;
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
        [Header("Controller")]
        [SerializeField] protected Renderer _renderBounds;
        [SerializeField] protected GameObject pathFinderTiles;
        [SerializeField] protected Rigidbody _rb;
        // [SerializeField] protected float movementSpeed;
        // [SerializeField] protected float rotationSpeed;
        public OctreeNode<Controller> CurrentNode { get; set; }
        public Renderer RenderBounds => _renderBounds == null ? _renderBounds : _renderBounds = GetComponent<Renderer>();
        public Rigidbody Rb => _rb == null ? _rb : _rb = GetComponent<Rigidbody>();
        public Transform RbTransform => _rb.transform;

        public void Start()
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
