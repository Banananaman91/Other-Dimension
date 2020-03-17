using System.Collections.Generic;
using System.Linq;
using Controllers;
using UnityEngine;

namespace GameOctree
{
    public class OctreeNode<T>
    {
        public OctreeNode<T> ParentNode { get; set; }
        private OctreeNode<T>[] _subNodes;
        List<T> _data = new List<T>();
        private Vector3 _position;
        private float _size;
        private int _depth;

        public OctreeNode(Vector3 pos, float size, int depth, OctreeNode<T> parentNode = null) // node constructor
        {
            ParentNode = parentNode;
            _position = pos;
            _size = size;
            _depth = depth;
        }

        public IEnumerable<OctreeNode<T>> Nodes => _subNodes;
        public Vector3 Position => _position;
        public float Size => _size;

        public List<T> Data => _data;

        public void AddData(T data) // add data object to list for storing in node, used later for retrieval
        {
            _data.Add(data);
        }

        public void RemoveData(T data) // removes data from node, used when data object has left the node (moving objects)
        {
            _data.Remove(data);
        }

        public void RemoveNode(OctreeNode<T> target)
        {
            for (int i = 0; i < _subNodes.Length; i++)
            {
                if (_subNodes[i] == target) _subNodes[i] = null;
            }
        }

        public List<T> ReturnData() // returns the data list
        {
            return _data;
        }

        public OctreeNode<T> SubDivide(Vector3 targetPosition, T value, int depth = 0) // subdivides nodes for octree
        {
            if (depth == 0) return this; // if we have reached the lowest depth, return as leaf node
            var indexPosition = GetIndexOfPosition(targetPosition, _position); // find index of position, represented by enum index
            bool any = false;

            if (_subNodes != null)
            {
                any = _subNodes.Any(node => node == null);
            }

            if (_subNodes == null || any)
            {
                _subNodes = new OctreeNode<T>[8];

                for (int i = 0; i < _subNodes.Length; i++)
                {
                    
                    Vector3 newPos = _position;
                    // bitewise operator used to reposition node centre points by a quarter in each direction, creating 8 node positions
                    if ((i & 4) == 4) 
                    {
                        newPos.y += _size * 0.25f;
                    }
                    else
                    {
                        newPos.y -= _size * 0.25f;
                    }

                    if ((i & 2) == 2)
                    {
                        newPos.x += _size * 0.25f;
                    }
                    else
                    {
                        newPos.x -= _size * 0.25f;
                    }

                    if ((i & 1) == 1)
                    {
                        newPos.z += _size * 0.25f;
                    }
                    else
                    {
                        newPos.z -= _size * 0.25f;
                    }

                    _subNodes[i] = new OctreeNode<T>(newPos, _size * 0.5f, depth - 1, this);
                }
            }
            
            return _subNodes[indexPosition].SubDivide(targetPosition, value, depth - 1); // subdivide further
        }

        public OctreeNode<T> NodeSearch(Vector3 targetPosition, int depth = 0) // search through node index for leaf nodes, recursive
        {
            if (depth == 0) return this; // returns at final depth
            if (_subNodes == null) return this;

            var indexPosition = GetIndexOfPosition(targetPosition, _position);
            return _subNodes[indexPosition] == null ? this : _subNodes?[indexPosition].NodeSearch(targetPosition, depth - 1);
        }

        public void RemoveNodes(int depth = 0)
        {
            if (depth == 0 && _data.Count == 0) ParentNode.RemoveNode(this);
            else if (_subNodes == null && _data.Count == 0) ParentNode.RemoveNode(this);
            else
            {
                foreach (var subNode in _subNodes)
                {
                    RemoveNodes(depth - 1);
                }
            }
        }

        public bool IsLeaf() // used by OctreeComponent when drawing nodes, checks only for lowest depth leafs
        {
            return _depth == 0;
        }
        
        private int GetIndexOfPosition(Vector3 lookupPosition, Vector3 nodePosition) // finds node position on index
        {
            var index = 0;

            index |= lookupPosition.y > nodePosition.y ? 4 : 0;
            index |= lookupPosition.x > nodePosition.x ? 2 : 0;
            index |= lookupPosition.z > nodePosition.z ? 1 : 0;
            
            return index;
        }
    }
}
