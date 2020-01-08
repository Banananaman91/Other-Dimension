using System;
using Controllers.States;
using Random = UnityEngine.Random;


namespace Controllers
{
    public class AiMaster : Controller
    {
        protected bool PathFinderReady => Pathfinder != null;
        public AiState State = AiState.Idle;
        protected StateChange StateChange => new StateChange(this);

        public virtual void Update()
        {
            switch (State)
            {
                case AiState.Idle:
                case AiState.Moving:
                case AiState.FindingTarget:
                case AiState.Alert:
                case AiState.Attack:
                case AiState.Block:
                case AiState.Capture:
                case AiState.Chase:
                case AiState.Maneuver:
                case AiState.Strike:
                case AiState.FindingPath:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
