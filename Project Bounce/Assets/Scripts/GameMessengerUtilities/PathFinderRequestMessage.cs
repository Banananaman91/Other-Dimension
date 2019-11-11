namespace GameMessengerUtilities
{
    public struct PathFinderRequestMessage
    {
        public Controller RequestingComponent { get; }

        public PathFinderRequestMessage(Controller requestingComponent) => RequestingComponent = requestingComponent;
        
    }
}
