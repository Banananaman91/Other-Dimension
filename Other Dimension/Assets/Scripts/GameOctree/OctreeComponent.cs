using System;
using System.Collections.Generic;
using Controllers;
using GameMessengerUtilities;
using PathFinding;
using UnityEngine;

namespace GameOctree
{
    public class OctreeComponent : MonoBehaviour
    {
        public float size = 5;

        public int depth = 2;

        public IList<Controller> Points => _avoidanceInstance.Objects;

        private Octree<Controller> _octree;
        private ObjectAvoidance _avoidanceInstance;
        
        private void Awake()
        {
            MessageBroker.Instance.RegisterMessageOfType<ObjectRequestMessage>(OnObjectAvoidanceRequestMessage);
            _avoidanceInstance = new ObjectAvoidance();
        }
        
        private void OnObjectAvoidanceRequestMessage(ObjectRequestMessage message) => message.RequestingComponent.ObjectInitialise(_avoidanceInstance);
        // Start is called before the first frame update
        void Start()
        {
            _octree = new Octree<Controller>(transform.position, size, depth);
        }
        

        private void OnDrawGizmos()
        {
            if (Points != null)
            {
                var octree = new Octree<Controller>(transform.position, size, depth);
                foreach (var point in Points)
                {
                    octree.Insert(point, point.transform.position);
                }
        
                DrawNode(octree.GetRoot());
            }
        }

        private void DrawNode(OctreeNode<Controller> node)
        {
            if (!node.IsLeaf())
            {
                if (node.Nodes != null)
                {
                    foreach (var subNode in node.Nodes)
                    {
                        if (subNode != null) DrawNode(subNode);
                    }
                }
            }

            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(node.Position, Vector3.one * node.Size);
        }
    }
}
