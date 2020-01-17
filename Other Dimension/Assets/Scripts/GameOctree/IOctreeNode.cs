using UnityEngine;

namespace GameOctree
{
    public interface IOctreeNode
    {
        Bounds BoundingArea { get; set; }
        BoundingSphere BoundingSphere { get; set; }
    }
}