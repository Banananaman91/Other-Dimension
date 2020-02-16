namespace Controllers
{
    public interface IState
    {
        void ToMoveState();
        void ToFindTargetState();
        void ToFindPathState();
        void ToIdleState();
        void ToAttackState();
        void ToCaptureState();
        void ToChaseState();
        void ToBlockState();
        void ToAlertState();
        void ToManeuverState();
        void ToStrikeState();
    }
}
