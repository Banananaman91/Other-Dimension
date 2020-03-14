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
            _transformDirection = transform.forward;
            var position = transform.position;
            _laserVisual.SetPosition(0, position);
            _laserVisual.SetPosition(1, position);
            Physics.Raycast(transform.position, transform.forward, out _hit, _distance, -10);
            _laserVisual.SetPosition(1, position + _transformDirection * _distance);
            if (!_hit.collider)
            {
                if (_rayReceiver == null) return;
                _rayReceiver.LaserColour -= _laserVisual.startColor;
                _rayReceiver = null;
                _addedColour = false;
                return;
            }
            _rayReceiver = _hit.collider.gameObject.GetComponent<IRayReceiver>();
            _laserVisual.SetPosition(1, _hit.point);
            if (_rayReceiver == null) return;
            if (!_addedColour)
            {
                _rayReceiver.LaserColour += _laserVisual.startColor;
                _addedColour = true;
            }
            _rayReceiver.HitWithRay(this);
        }
    }
}
