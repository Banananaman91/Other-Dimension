using System;
using Controllers;
using Puzzle.Laser;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Ray = Puzzle.Laser.Ray;

namespace GamePhysics
{
    public class WhiteHole : MonoBehaviour, IRayReceiver
    {
        [SerializeField] private PlayerController _player;
        [SerializeField] private Text _levelText;
        public float gravity = -12;
        public List<Color> _colours = new List<Color>();
        private List<Ray> _lasers = new List<Ray>();
        private int _colourCount;
        private int _currentColour;
        private Color[] _colourChoices = { Color.red, Color.blue, Color.green, Color.red + Color.blue, Color.red + Color.green, Color.blue + Color.green };

        public Color LaserColour { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        private void Awake()
        {
            ColourSequence();
            if (_levelText)
            {
                _levelText.text = "Level: " + _colourCount;
                _levelText.color = _colours[_currentColour];
            }
        }

        private void CheckRay(int rayCount)
        {
            if (_lasers[rayCount].LaserVisual.startColor != _colours[rayCount]) ResetColours();
            else ColourSequence();
            _currentColour++;
        }
        
        public void HitWithRay(Ray ray)
        {
            if (!_lasers.Contains(ray))
            {
                _lasers.Add(ray);
                CheckRay(_currentColour);
            }
        }

        public void NotHitWithRay()
        {
            throw new System.NotImplementedException();
        }

        public void ColourSequence()
        {
            _colours.Clear();
            _currentColour = 0;
            for (var i = 0; i <= _colourCount; i++)
            {
                var colourChoice = Random.Range(0, _colourChoices.Length);
                var newColour = _colourChoices[colourChoice];
                _colours.Add(newColour);
            }
            _colourCount++;
            if (_levelText)
            {
                _levelText.color = _colours[_currentColour];
                _levelText.text = "Level: " + _colourCount;
            }

            _player.DisplayColour(this, _currentColour);
        }

        private void ResetColours()
        {
            _colourCount = 0;
            ColourSequence();
        }

        public void Repel(Controller playerTransform, int additive = 0)
        {
            var transform1 = playerTransform.transform;
            Vector3 gravityUp = (transform.position - transform1.position).normalized;
            Vector3 localUp = transform1.up;

            if (additive == 0) playerTransform.Rb.AddForce(gravityUp * gravity);
            else playerTransform.Rb.AddForce(gravityUp * (gravity + additive));

            var rotation = playerTransform.transform.rotation;
            Quaternion targetRotation = Quaternion.FromToRotation(localUp, gravityUp) * rotation;
            rotation = Quaternion.Slerp(rotation, targetRotation, 50f * Time.deltaTime);
            playerTransform.transform.rotation = rotation;
        }
    }
}
