using System.Collections.Generic;
using UnityEngine;

namespace GameOctree
{
    public class Octree<T> where T : IOctreeNode
    {
        private Octree<T> _parent;
        private List<T> _objects;
        private Bounds _region;
        Octree<T>[] _childNode = new Octree<T>[8];
        private byte _activeNodes;
        private int _minSize = 1;
        private bool _treeReady = false;
        private bool _treeBuilt = false;
    }
}
