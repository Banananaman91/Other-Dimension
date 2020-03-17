using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


namespace Terrain
{
    public class ObjectCreation : MonoBehaviour
    {
        [SerializeField] private GameObject[] _feature;
        [SerializeField] private GameObject[] _terrainObjects;
        [SerializeField] private GameObject _spawnPoint;
        [SerializeField] private int _puzzleCount;
        [SerializeField] private int _objectCount;
        [SerializeField] private Collider _whiteHoleCollider;
        private GameObject _spawnedObject;
        private List<GameObject> _objects = new List<GameObject>(); 

        public IEnumerator CreateObjects()
        {
            if (_whiteHoleCollider.enabled) _whiteHoleCollider.enabled = false;
            for (var i = 0; i < _puzzleCount; i++)
            {
                var itemNumber = Random.Range(0, _feature.Length);
                var go = Instantiate(_feature[itemNumber], _spawnPoint.transform);
                _objects.Add(go);
            }

            for (int i = 0; i < _objectCount; i++)
            {
                var itemNumber = Random.Range(0, _terrainObjects.Length);
                var go = Instantiate(_terrainObjects[itemNumber]);
                _objects.Add(go);
            }

            if (!_whiteHoleCollider.enabled) _whiteHoleCollider.enabled = true;
            yield return null;
        }
    }
}
