using System;
using Menu;
using UnityEngine;

namespace Controllers
{
    public class CameraOrbit : MonoBehaviour
    { 
        [SerializeField] private GameObject _target;
        [SerializeField] private GameObject _menuTarget;
        [SerializeField] private GameObject[] _targetCycles;
        [SerializeField] private int _heightAbovePlayer;
        [SerializeField] private float _upDistance;
        [SerializeField] private float _backDistance;
        [SerializeField] private float _trackingSpeed;
        [SerializeField] private MenuBehaviour _menu;
        private int _selection = 1;
        private Vector3 _targetVector3;
        private Quaternion _targetQuaternion;
        private Transform TargetTransform { get; set; }
        private Vector3 _aboveVector3;
        private void FixedUpdate()
        {
            if (!_menu.GameActive) TargetTransform = _targetCycles[0].transform;
            //TargetTransform = _menu.GameActive ? _targetCycles[1].transform : _targetCycles[0].transform;
            
            if (_menu.GameActive)
            {
                if (TargetTransform == _targetCycles[0].transform) TargetTransform = _targetCycles[_selection].transform;
                if (Input.GetKeyDown(KeyCode.Tab))
                {
                    if (_selection > _targetCycles.Length - 1) _selection = 1;
                    TargetTransform = _targetCycles[_selection].transform;
                    _selection++;
                }
            }
            
            _targetVector3 = TargetTransform.position - TargetTransform.forward * _backDistance + TargetTransform.up * _upDistance;
            _aboveVector3 = TargetTransform.position + TargetTransform.up * _heightAbovePlayer;
            transform.position = Vector3.Lerp (transform.position, _targetVector3, _trackingSpeed * Time.deltaTime);
            transform.LookAt(_aboveVector3, TargetTransform.up);
        }
    }
}
