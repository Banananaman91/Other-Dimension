using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameOctree
{
    public enum OctreeIndex // unused, but represents binary index for OctreeNode when getting node position index
    {
        LowerLeftFront = 0, //000,
        LowerRightFront = 2, //010,
        LowerRightBack = 3, //011,
        LowerLeftBack = 1, //001,
        UpperLeftFront = 4, //100,
        UpperRightFront = 6, //110,
        UpperRightBack = 7, //111,
        UpperLeftBack = 5, //101,
    }
    public class Octree<T>
    {
        private OctreeNode<T> _node;
        private int _depth;
        private IList<T> _objectList;
        
        public Octree(Vector3 position, float size, int depth)
        {
            _node = new OctreeNode<T>(position, size, depth); // create root node
            _depth = depth; // set depth
        }

        public OctreeNode<T> Insert(T value, Vector3 position)
        {
            var leafNode = _node.SubDivide(position, value, _depth); // subdivide nodes based on individual positioning. This is recursive and is also called for each individual object
            leafNode?.AddData(value); // add object to data list at leaf nodes
            return leafNode;
        }

        public OctreeNode<T> NodeCheck(Vector3 position) // used for checking nodes, returns node to be checked
        {
            var leafNode = _node.NodeSearch(position, _depth);
            return leafNode;
        }

        public IList<T> ObjectCheck(Vector3 position) // searches for leaf node and returns data list for that node
        {
            //retrieve objects from leafnode at current position
            var leafNode = _node.NodeSearch(position, _depth);
            var data = leafNode.ReturnData();
            return data;
        }

        public OctreeNode<T> GetRoot() // used by DrawGizmo in OctreeComponent, only used when debugging Octree visually
        {
            return _node;
        }
    }
}
