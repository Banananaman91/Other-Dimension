using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Terrain
{
    public class ObjectPlacer : MonoBehaviour
    {
        private Transform _parent => GetComponentInParent<Transform>();
        private void Awake()
        {
            var scale = Random.Range(1, 20);
            transform.localScale = new Vector3(scale, scale, scale);
            var length = 1000;
            var direction = Random.onUnitSphere;
            Physics.Raycast (transform.position, direction * length, out var hit, Mathf.Infinity, 9);
            transform.position = hit.point;
            transform.rotation = Quaternion.FromToRotation(Vector3.up, _parent.transform.position);
        }
    }
}
