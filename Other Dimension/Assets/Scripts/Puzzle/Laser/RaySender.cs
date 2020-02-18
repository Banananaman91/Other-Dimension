using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Puzzle.Laser
{
    public class RaySender : MonoBehaviour
    {
        [SerializeField] private LineRenderer _laserVisual;
        [SerializeField] private ParticleSystem _laserParticle;
        private Color[] _startColours = new Color[] {Color.red, Color.green, Color.blue, };
        private float _distance = 100;
        private IRayReceiver _rayReceiver;
        private RaycastHit _hit;
        private bool _addedColour;

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
            Physics.Raycast(transform.position, transform.forward, out _hit, _distance);
            _laserVisual.SetPosition(1, new Vector3(0, 0, _distance));
            if (!_hit.collider)
            {
                if (_rayReceiver == null) return;
                Debug.Log(_rayReceiver.LaserColour);
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
