using UnityEngine;

namespace Controllers.States
{
    public class StateChange : IState
    {
        private readonly AiMaster _aiController;

        public StateChange(AiMaster aiController)
        {
            _aiController = aiController;
        }
        
        public void ToMoveState()
        {
            _aiController.State = AiState.Moving;
        }

        public void ToFindTargetState()
        {
            _aiController.State = AiState.FindingTarget;
        }

        public void ToFindPathState()
        {
            _aiController.State = AiState.FindingPath;
        }

        public void ToIdleState()
        {
            _aiController.State = AiState.Idle;
        }

        public void ToAttackState()
        {
            _aiController.State = AiState.Attack;
        }

        public void ToCaptureState()
        {
            _aiController.State = AiState.Capture;
        }

        public void ToChaseState()
        {
            _aiController.State = AiState.Chase;
        }

        public void ToBlockState()
        {
            _aiController.State = AiState.Block;
        }

        public void ToAlertState()
        {
            _aiController.State = AiState.Alert;
        }

        public void ToManeuverState()
        {
            _aiController.State = AiState.Maneuver;
        }

        public void ToStrikeState()
        {
            _aiController.State = AiState.Strike;
        }
    }
}
