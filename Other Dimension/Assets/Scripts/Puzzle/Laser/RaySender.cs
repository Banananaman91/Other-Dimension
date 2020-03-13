using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Puzzle.Laser
{
    public class RaySender : Ray
    {
        private void Awake()
        {
            var colourChoice = Random.Range(0, _startColours.Length);
            _laserVisual.startColor = _startColours[colourChoice];
            _laserVisual.endColor = _startColours[colourChoice];
            var laserParticleMain = _laserParticle.main;
            laserParticleMain.startColor = _startColours[colourChoice];
        }

        private void FixedUpdate()
        {
            Physics.Raycast(transform.position, transform.forward, out _hit, _distance, -10);
            _laserVisual.SetPosition(1, new Vector3(0, 0, _distance));
            if (!_hit.collider)
            {
                if (_rayReceiver == null) return;
                _rayReceiver.LaserColour -= _laserVisual.startColor;
                _rayReceiver = null;
                _addedColour = false;
                return;
            }
            var distance = Vector3.Distance(transform.position, _hit.point);
            _laserVisual.SetPosition(1, new Vector3(0, 0, distance * 2));
            _rayReceiver = _hit.collider.gameObject.GetComponent<IRayReceiver>();
            if (_rayReceiver == null) return;
            if (!_addedColour)
            {
                _rayReceiver.LaserColour += _laserVisual.startColor;
                _addedColour = true;
            }
            _rayReceiver.HitWithRay();
        }
    }
}
