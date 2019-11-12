using System;
using UnityEngine;

namespace Controllers
{
    public class AntiGravity : MonoBehaviour
    {
        [SerializeField] private BouncyBall _player;
        private float _gravity = 9.08f;
        private void FixedUpdate()
        {
            Vector3 Direction = _player.transform.position - transform.position;
            _player.Rb.AddForce(Direction * Physics.gravity.y / 50);
        }
    }
}
