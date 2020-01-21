using System.Collections.Generic;
using UnityEngine;

namespace GameOctree
{
    public class Octree<T> where T : IOctreeNode, new()
    {
        private T _parent;
        private List<T> _objects;
        private Bounds _region;
        private int _minSize = 1;
        private bool _treeReady = false;
        private bool _treeBuilt = false;

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

        private void Insert(T target, List<T> updateList, Vector3 min, Vector3 max)
        {
            var bounds = CreateBounds(min, max); // create array of bounding boxes to be checked against
            
            for (int i = 0; i < bounds.Length; i++) // for all the bounds
            {
                foreach (var octreeNode in target.ChildNodes)// check the children of target node
                {
                    var child = (T) octreeNode;
                    if (!bounds[i].Contains(child.BoundingArea.size)) continue; // if the bounds doesn't contain the child, continue
                    child.Region = bounds[i];
                    if (child.ChildNodes == null) // if the child has no children, it is an end node.
                    {
                        updateList.Add(child);
                        continue;
                    }
                    
                    Insert(child, updateList, bounds[i].min, bounds[i].max); // Recursive call down through child nodes
                }
            }
        }
    }
}
