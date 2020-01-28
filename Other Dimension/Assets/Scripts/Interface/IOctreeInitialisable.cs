using Controllers;
using GameOctree;

namespace Interface
{
    public interface IOctreeInitialisable
    {
        void OctreeInitialise(Octree<Controller> objectAvoidance);
    }
}
