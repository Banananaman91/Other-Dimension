using System;
using UnityEngine;

namespace GameOctree
{
    public class OctreeComponent : MonoBehaviour
    {
        public float size = 5;

        public int depth = 2;
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        private void OnDrawGizmos()
        {
            var octree = new Octree<int>(this.transform.position, size, depth);
            DrawNode(octree.GetRoot());
        }

        private void DrawNode(Octree<int>.OctreeNode<int> node)
        {
            if (node.IsLeaf())
            {
                Gizmos.color = Color.green;
            }
            else
            {
                Gizmos.color = Color.blue;
                foreach (var subNode in node.Nodes)
                {
                    DrawNode(subNode);
                }
            }
            
            Gizmos.DrawWireCube(node.Position, Vector3.one * node.Size);
        }
    }
}
