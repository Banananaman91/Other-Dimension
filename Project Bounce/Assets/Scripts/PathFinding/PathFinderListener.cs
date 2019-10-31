﻿using GameMessengerUtilities;
using UnityEngine;

namespace PathFinding
{
    public class PathFinderListener : MonoBehaviour
    {
        private void Awake() => GameMessengerUtilities.MessageBroker.Instance.RegisterMessageOfType<PathFinderRequestMessage>(OnPathFinderComponentRequestMessage);
        
        private void OnPathFinderComponentRequestMessage(PathFinderRequestMessage message) => message.RequestingComponent.PathInitialise(new PathFinder());
    }
}
