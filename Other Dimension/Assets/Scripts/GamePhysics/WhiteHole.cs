using System;
using Controllers;
using Puzzle.Laser;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using Ray = Puzzle.Laser.Ray;

namespace GamePhysics
{
    public class WhiteHole : MonoBehaviour, IRayReceiver
    {
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
        }

        private void CheckRay(int rayCount)
        {
            Debug.Log(_lasers[rayCount].LaserVisual.startColor != _colours[rayCount]
                ? "You're a failure"
                : "Pretty good");
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
        }

        public void Repel(PlayerController playerTransform)
        {
            Vector3 gravityUp = (transform.position - playerTransform.PlayerTransform.position).normalized;
            Vector3 localUp = playerTransform.PlayerTransform.up;

            playerTransform.Rb.AddForce(gravityUp * gravity);

            Quaternion targetRotation = Quaternion.FromToRotation(localUp, gravityUp) * playerTransform.PlayerTransform.rotation;
            playerTransform.PlayerTransform.rotation = Quaternion.Slerp(playerTransform.PlayerTransform.rotation, targetRotation, 50f * Time.deltaTime);
        }
    }
}
