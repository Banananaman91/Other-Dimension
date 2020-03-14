using System;
using UnityEngine;

namespace Puzzle
{
    public class Allign : MonoBehaviour
    {
        [SerializeField] private Transform _centre;
        [SerializeField] private Transform _myPosition;
        [SerializeField] private Transform _spawn;

        private void Start()
        {
            
            _myPosition.forward = _myPosition.localPosition - _centre.localPosition;
        }
    }
}
