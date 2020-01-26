using System.Collections.Generic;
using UnityEngine;

namespace GameOctree
{
    public class OctreeNode<T>
    {
        private OctreeNode<T>[] _subNodes;
        T _data;
        private Vector3 _position;
        private float _size;
        private int _depth;

        public OctreeNode(Vector3 pos, float size, int depth)
        {
            _position = pos;
            _size = size;
            _depth = depth;
        }

        public IEnumerable<OctreeNode<T>> Nodes => _subNodes;
        public Vector3 Position => _position;
        public float Size => _size;

        public T Data
        {
            get { return _data; }
            internal set { _data = value; }
        }

        public OctreeNode<T> SubDivide(Vector3 targetPosition, T value, int depth = 0)
        {
            if (depth == 0) return this;
            var indexPosition = GetIndexOfPosition(targetPosition, _position);
            IList<T> subList = new List<T>();
            if (_subNodes == null)
            {
                _subNodes = new OctreeNode<T>[8];

                for (int i = 0; i < _subNodes.Length; i++)
                {
                    
                    Vector3 newPos = _position;
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

                    _subNodes[i] = new OctreeNode<T>(newPos, _size * 0.5f, depth - 1);
                }
            }

            return _subNodes[indexPosition].SubDivide(targetPosition, value, depth - 1);
        }

        public bool IsLeaf()
        {
            return _depth == 0;
        }
        
        private int GetIndexOfPosition(Vector3 lookupPosition, Vector3 nodePosition)
        {
            var index = 0;

            index |= lookupPosition.y > nodePosition.y ? 4 : 0;
            index |= lookupPosition.x > nodePosition.x ? 2 : 0;
            index |= lookupPosition.z > nodePosition.z ? 1 : 0;
            
            return index;
        }
        
        private Bounds _region;
        
        private Bounds[] CreateBounds(Vector3 min, Vector3 max) //create bounds array for checking
        {
            var bounds = new Bounds[8];
            var half = (max - min) / 2;
            var centre = min + half;
            //Create 8 bounding boxes inside original bounding box
            bounds[0] = new Bounds(min, centre);
            bounds[1] = new Bounds(new Vector3(centre.x, min.y, min.z), new Vector3(max.x, centre.y, centre.z));
            bounds[2] = new Bounds(new Vector3(centre.x, min.y, centre.z), new Vector3(max.x, centre.y, max.z));
            bounds[3] = new Bounds(new Vector3(min.x, min.y, centre.z), new Vector3(centre.x, centre.y, max.z));
            bounds[4] = new Bounds(new Vector3(min.x, centre.y, min.z), new Vector3(centre.x, max.y, centre.z));
            bounds[5] = new Bounds(new Vector3(centre.x, centre.y, min.z), new Vector3(max.x, max.y, centre.z));
            bounds[6] = new Bounds(centre, max);
            bounds[7] = new Bounds(new Vector3(min.x, centre.y, centre.z), new Vector3(centre.x, max.y, max.z));
            return bounds;
        }
    }
}
