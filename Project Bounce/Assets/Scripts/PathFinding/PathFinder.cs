using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Controllers;
using GameMessengerUtilities;
using Interface;
using UnityEngine;

namespace PathFinding
{
    public class PathFinder : IPathfinder, IObjectAvoidanceInitialisable
    {
        private List<Vector3> _pathToFollow = new List<Vector3>();
        private ObjectAvoidance _avoidance;

        public IEnumerator FindPath(float stepValue, Vector3 startPosition, Vector3 targetPosition, float movementRadius, Action<IEnumerable<Vector3>> onCompletion)
        {
            MessageBroker.Instance.SendMessageOfType(new ObjectRequestMessage(this));
            List<Location> openList = new List<Location>();
            List<Location> closedList = new List<Location>();
            Location currentLocation;
            Location startLocation = new Location(startPosition, targetPosition, null);
            var isObjectMoving = _avoidance.Objects.First(x => x.transform.position == startPosition);

            var adjacentSquares = new List<Location>();
            openList.Add(startLocation);

            while (openList.Count > 0)
            {
                // Find square with lowest F value
                var lowestFScore = openList.Min(x => x.F);
                currentLocation = openList.First(x => x.F == lowestFScore);

                closedList.Add(currentLocation);
                openList.Remove(currentLocation);

                var distance = Vector3.Distance(currentLocation.PositionInWorld, targetPosition);

                if (distance < stepValue || distance < 1)
                {
                    if (stepValue > 1)
                    {
                        stepValue /= 2;
                    }
                    else
                    {
                        targetPosition = currentLocation.PositionInWorld;
                    }
                }
                if (closedList.Any(x => x.PositionInWorld == targetPosition))
                {
                    break;
                }

                adjacentSquares.Clear();
                adjacentSquares = GetAdjacentSquares3D(currentLocation, targetPosition, isObjectMoving, stepValue);

                foreach (var adjacentSquare in adjacentSquares)
                {
                    if (closedList.Any(x => x.PositionInWorld == adjacentSquare.PositionInWorld)) continue;

                    if (openList.All(x => x.PositionInWorld != adjacentSquare.PositionInWorld)) openList.Add(adjacentSquare);

                    else if (adjacentSquare.F < openList.First(x => x.PositionInWorld == adjacentSquare.PositionInWorld).F) openList.First(x => x.PositionInWorld == adjacentSquare.PositionInWorld).Parent = adjacentSquare;
                }

                yield return null;
            }

            _pathToFollow.Clear();

            var current = closedList.Last();

            _pathToFollow.Add(current.PositionInWorld);
            
            do
            {
                _pathToFollow.Add(current.Parent.PositionInWorld);
                current = current.Parent;
            } while (!_pathToFollow.Contains(startPosition));

            _pathToFollow.Reverse();

            onCompletion(_pathToFollow);
        }

        private List<Location> GetAdjacentSquares3D(Location point, Vector3 target, Controller isObjectMoving, float stepValue)
        {
            List<Location> returnList = new List<Location>();

            for (var xIndex = point.PositionInWorld.x - stepValue; xIndex <= point.PositionInWorld.x + stepValue; xIndex+=stepValue)
            {
                for (var yIndex = point.PositionInWorld.y - stepValue; yIndex <= point.PositionInWorld.y + stepValue; yIndex+=stepValue)
                {
                    for (var zIndex = point.PositionInWorld.z - stepValue; zIndex <= point.PositionInWorld.z + stepValue; zIndex+=stepValue)
                    {
                        var adjacentVector = new Location(new Vector3(xIndex, yIndex, zIndex), target,
                            point);
                        bool isIntersecting = _avoidance.Objects
                            .Where(x => x != isObjectMoving && Vector3.Distance(x.transform.position, adjacentVector.PositionInWorld) <=
                                        Vector3.Distance(point.PositionInWorld, target)).Any(x => x.RenderBounds.bounds.Contains(adjacentVector.PositionInWorld));
                        if (!isIntersecting) returnList.Add(adjacentVector);
                    }
                }
            }

            return returnList;
        }
        
        public void ObjectInitialise(ObjectAvoidance objectAvoidance)
        {
            _avoidance = objectAvoidance;
        }
    }
}
