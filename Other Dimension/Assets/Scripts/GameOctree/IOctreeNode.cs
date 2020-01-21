using UnityEngine;

namespace GameOctree
{
    public interface IOctreeNode
    {
        Bounds BoundingArea { get; set; }
        Bounds Region { get; set; }
        IOctreeNode[] ChildNodes { get; set; }
        
    }
}