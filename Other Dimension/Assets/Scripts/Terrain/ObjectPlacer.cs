using System;
using Controllers;
using Puzzle.Builder;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Terrain
{
    public class ObjectPlacer : MonoBehaviour
    {
        [SerializeField] private PuzzleConstructor _puzzleConstructor;
        private Transform Parent => GetComponentInParent<Transform>();
        private void Awake()
        {
            _puzzleConstructor.Origin = Parent;
            var length = 1000;
            var direction = Random.onUnitSphere;
            Physics.Raycast (transform.position, direction * length, out var hit, Mathf.Infinity);
            transform.position = hit.point;
            _puzzleConstructor.Activate();
            transform.rotation = Quaternion.FromToRotation(-Vector3.up, Parent.transform.localPosition);
        }
    }
}
