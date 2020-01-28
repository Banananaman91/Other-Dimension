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

        public Octree<Controller> Octree => _octree;

        private void Awake()
        {
            MessageBroker.Instance.RegisterMessageOfType<ObjectRequestMessage>(OnObjectAvoidanceRequestMessage);
            _avoidanceInstance = new ObjectAvoidance();
            MessageBroker.Instance.RegisterMessageOfType<OctreeRequestMessage>(OnOctreeRequestMessage);
            _octree = new Octree<Controller>(transform.position, size, depth); // Instantiate Octree, sending in parameters for root node
            if (Points == null) return;
            foreach (var point in Points)
            {
                var position = point.transform.position;
                point.CurrentNode =  _octree.Insert(point, position); // insert all objects into Octree
            }
        }
        
        private void OnObjectAvoidanceRequestMessage(ObjectRequestMessage message) => message.RequestingComponent.ObjectInitialise(_avoidanceInstance);
        private void OnOctreeRequestMessage(OctreeRequestMessage message) => message.RequestingComponent.OctreeInitialise(_octree);

        private void Update()
        {
            if (Points == null) return;
            foreach (var point in Points)
            {
                var newNode = _octree.NodeCheck(point.transform.position); // check node for each point -- optimise to only check moving points
                if (point.CurrentNode == null)
                {
                    point.CurrentNode = _octree.Insert(point, point.transform.position);
                    continue;
                }
                if (newNode == point.CurrentNode) continue; //if it is still in the same node then continue
                point.CurrentNode.RemoveData(point); // if it is a new node, remove self from nodes data
                point.CurrentNode = _octree.Insert(point, point.transform.position); // set current node to the new node
            }
        }

        
        
        // private void OnDrawGizmos()
        // {
        //     if (Points != null)
        //     {
        //         // var octree = new Octree<Controller>(transform.position, size, depth);
        //         // foreach (var point in Points)
        //         // {
        //         //     octree.Insert(point, point.transform.position);
        //         // }
        //
        //         DrawNode(_octree.GetRoot());
        //     }
        // }
        
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
