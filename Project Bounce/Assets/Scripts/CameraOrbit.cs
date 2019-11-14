using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour
{ 
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _clamp;
    [SerializeField] private float _rotateSpeed;

    void LateUpdate()
     {
         if(Input.GetKey (KeyCode.E))
         {
             transform.RotateAround(_target.position, Vector3.up, _rotateSpeed * Time.deltaTime);
         }

         if(Input.GetKey (KeyCode.Q))
         {
             transform.RotateAround(_target.position, -Vector3.up, _rotateSpeed * Time.deltaTime);
         }
         
         transform.position = _clamp.position;
     }
}
