using System;
using System.Collections;
using Controllers;
using Puzzle.Builder;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Terrain
{
    public class ObjectPlacer : MonoBehaviour
    {
        [SerializeField] private PuzzleConstructor _puzzleConstructor;
        private RaycastHit _hit;
        private Transform Parent => GetComponentInParent<Transform>();
        private void Awake()
        {
            var length = 1000;
            
            while (!_hit.collider)
            {
                var direction = Random.onUnitSphere;
                Physics.Raycast (transform.position, direction * length, out _hit, Mathf.Infinity, -9);
            }
            transform.position = _hit.point;
        }

        private void Start()
        {
            if (_puzzleConstructor)
            {
                _puzzleConstructor.ConstructPuzzle();
            }

            if (!_puzzleConstructor)
            {
                var scale = Random.Range(1, 20);
                transform.localScale = new Vector3(scale, scale, scale);
            }
            transform.rotation = Quaternion.FromToRotation(-Vector3.up, Parent.transform.localPosition);
        }
    }
}
