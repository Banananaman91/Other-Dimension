using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameOctree
{
    public enum OctreeIndex
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
        
        public Octree(Vector3 position, float size, int depth)
        {
            _node = new OctreeNode<T>(position, size);
            _node.SubDivide(depth);
            
        }
        
        public class OctreeNode<T>
        {
            private OctreeNode<T>[] _subNodes;
            private IList<T> value;
            private Vector3 _position;
            private float _size;

            public OctreeNode(Vector3 pos, float size)
            {
                _position = pos;
                _size = size;
            }

            public IEnumerable<OctreeNode<T>> Nodes => _subNodes;
            public Vector3 Position => _position;
            public float Size => _size;

            public void SubDivide(int depth = 1)
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
                    _subNodes[i] = new OctreeNode<T>(newPos, _size * 0.5f);
                    if (depth > 0)
                    {
                        _subNodes[i].SubDivide(depth - 1);
                    }
                }
            }

            public bool IsLeaf()
            {
                return _subNodes == null;
            }
        }

        private int GetIndexOfPosition(Vector3 lookupPosition, Vector3 nodePosition)
        {
            var index = 0;

            index |= lookupPosition.y > nodePosition.y ? 4 : 0;
            index |= lookupPosition.x > nodePosition.x ? 2 : 0;
            index |= lookupPosition.z > nodePosition.z ? 1 : 0;
            
            return index;
        }
        
        public OctreeNode<T> GetRoot()
        {
            return _node;
        }
        
        // private Octree _parent;
        // private List<IOctreeNode> _objects;
        // private Octree[] _childNodes = new Octree[8];
        // private Bounds _region;
        //
        // private Bounds[] CreateBounds(Vector3 min, Vector3 max) //create bounds array for checking
        // {
        //     var bounds = new Bounds[8];
        //     var half = (max - min) / 2;
        //     var centre = min + half;
        //     //Create 8 bounding boxes inside original bounding box
        //     bounds[0] = new Bounds(min, centre);
        //     bounds[1] = new Bounds(new Vector3(centre.x, min.y, min.z), new Vector3(max.x, centre.y, centre.z));
        //     bounds[2] = new Bounds(new Vector3(centre.x, min.y, centre.z), new Vector3(max.x, centre.y, max.z));
        //     bounds[3] = new Bounds(new Vector3(min.x, min.y, centre.z), new Vector3(centre.x, centre.y, max.z));
        //     bounds[4] = new Bounds(new Vector3(min.x, centre.y, min.z), new Vector3(centre.x, max.y, centre.z));
        //     bounds[5] = new Bounds(new Vector3(centre.x, centre.y, min.z), new Vector3(max.x, max.y, centre.z));
        //     bounds[6] = new Bounds(centre, max);
        //     bounds[7] = new Bounds(new Vector3(min.x, centre.y, centre.z), new Vector3(centre.x, max.y, max.z));
        //     return bounds;
        // }
        //
        // private void Insert(Octree target, List<Octree> updateList, Vector3 min, Vector3 max)
        // {
        //     var bounds = CreateBounds(min, max); // create array of bounding boxes to be checked against
        //     
        //     for (int i = 0; i < bounds.Length; i++) // for all the bounds
        //     {
        //         foreach (var octreeNode in _childNodes)// check the children of target node
        //         {
        //             if (!bounds[i].Contains(octreeNode._region.size)) continue; // if the bounds doesn't contain the child, continue
        //             octreeNode._region = bounds[i];
        //             if (octreeNode._childNodes == null) // if the child has no children, it is an end node.
        //             {
        //                 updateList.Add(octreeNode);
        //                 continue;
        //             }
        //             
        //             Insert(octreeNode, updateList, bounds[i].min, bounds[i].max); // Recursive call down through child nodes
        //         }
        //     }
        // }

    }
}
