using System;
using UnityEngine;

namespace Controllers
{
    public class Orientation : MonoBehaviour
    {
        [SerializeField] private int _rotateSpeed;
        [SerializeField] private Transform _player;
        [SerializeField] private float _distance;

        private void FixedUpdate()
        {
            transform.position = _player.position;
            transform.Rotate(0, Input.GetAxis("Mouse X") * _rotateSpeed, 0, Space.Self);
            _player.forward = transform.forward;
        }
    }
}
