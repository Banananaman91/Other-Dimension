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
        private IList<T> _objectList;
        
        public Octree(Vector3 position, float size, int depth)
        {
            _node = new OctreeNode<T>(position, size, depth);
            _depth = depth;
        }

        public void Insert(T value, Vector3 position)
        {
            var leafNode = _node.SubDivide(position, value, _depth);
            leafNode.Data = value;
        }

        public OctreeNode<T> GetRoot()
        {
            return _node;
        }

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
