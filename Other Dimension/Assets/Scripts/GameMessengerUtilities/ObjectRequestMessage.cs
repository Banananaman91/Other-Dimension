using Interface;

namespace GameMessengerUtilities
{
    public struct ObjectRequestMessage
    {
        public IObjectAvoidanceInitialisable RequestingComponent { get; }

        public ObjectRequestMessage(IObjectAvoidanceInitialisable requestingComponent) => RequestingComponent = requestingComponent;
    }
}
