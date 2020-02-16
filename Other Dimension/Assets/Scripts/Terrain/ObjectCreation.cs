using System;
using System.Numerics;
using GamePhysics;
using Puzzle.Builder;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;

namespace Terrain
{
    public class ObjectCreation : MonoBehaviour
    {
        [SerializeField] private GameObject[] _feature;
        [SerializeField] private GameObject _spawnPoint;
        [SerializeField] private int _objectCount;
        private GameObject _spawnedObject;
        

        private void Start()
        {
            for (var i = 0; i < _objectCount; i++)
            {
                var itemNumber = UnityEngine.Random.Range(0, _feature.Length);
                Instantiate(_feature[itemNumber], _spawnPoint.transform);
            }
        }
    }
}
