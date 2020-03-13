using System;
using System.Collections;
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
        
        private void Start()
        { 
            StartCoroutine(CreateObjects());
        }

        private IEnumerator CreateObjects()
        {
            if (_whiteHoleCollider.enabled) _whiteHoleCollider.enabled = false;
            for (var i = 0; i < _puzzleCount; i++)
            {
                var itemNumber = Random.Range(0, _feature.Length);
                Instantiate(_feature[itemNumber], _spawnPoint.transform);
            }

            for (int i = 0; i < _objectCount; i++)
            {
                var itemNumber = Random.Range(0, _terrainObjects.Length);
                Instantiate(_terrainObjects[itemNumber]);
            }

            if (!_whiteHoleCollider.enabled) _whiteHoleCollider.enabled = true;
            yield return null;
        }
    }
}
