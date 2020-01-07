using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interface
{
    public interface IPathfinder
    {
        IEnumerator FindPath(float stepValue, Vector3 startPosition, Vector3 targetPosition, float movementRadius, Action<IEnumerable<Vector3>> onCompletion);
    }
}