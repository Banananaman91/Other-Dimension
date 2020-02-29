using System;
using System.Collections.Generic;
using Controllers;
using UnityEngine;

namespace Puzzle.Laser
{
    public class RayGoal : RayMaster, IRayInteract
    {
        [SerializeField] private GoalState _currentState;
        private Color _finalColour;
        private bool _freezeColour;

        private void Update()
        {
            switch (_currentState)
            {
                case GoalState.Awaiting:
                    break;
                case GoalState.Fired:
                    Fired();
                    break;
            }
        }

        private void Fired()
        {
            if (!_freezeColour && _finalColour != LaserColour) {
                _finalColour = LaserColour;
                _freezeColour = true;
            }
            _laserVisual.startColor = _finalColour;
            _laserVisual.endColor = _finalColour;
            var laserParticleMain = _laserParticle.main;
            laserParticleMain.startColor = _finalColour;
        }

        public void RayInteraction(PlayerController player)
        {
            _goalActive = true;
            _currentState = GoalState.Fired;
        }
    }
}
