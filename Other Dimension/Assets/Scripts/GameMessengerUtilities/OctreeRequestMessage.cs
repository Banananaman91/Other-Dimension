using Interface;

namespace GameMessengerUtilities
{
    public struct OctreeRequestMessage 
    {
        public IOctreeInitialisable RequestingComponent { get; }

        public OctreeRequestMessage(IOctreeInitialisable requestingComponent) => RequestingComponent = requestingComponent;
    }
}
