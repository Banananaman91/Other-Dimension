using Controllers;
using Puzzle.Laser;
using System.Collections.Generic;
using UnityEngine;

namespace GamePhysics
{
    public class WhiteHole : MonoBehaviour, IRayReceiver
    {
        public float gravity = -12;
        private List<Color> _colours;
        private int _colourCount;
        private int _currentColour;
        private Color[] _colourChoices = { Color.red, Color.blue, Color.green, Color.red + Color.blue, Color.red + Color.green, Color.blue + Color.green };

        public Color LaserColour { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void HitWithRay()
        {
            throw new System.NotImplementedException();
        }

        public void NotHitWithRay()
        {
            throw new System.NotImplementedException();
        }

        public void ColourSequence()
        {
            _colours.Clear();
            _colourCount++;
            for (var i = 0; i <= _colourCount; i++)
            {
                var colourChoice = Random.Range(0, _colourChoices.Length);
                var newColour = _colourChoices[colourChoice];
            }
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
