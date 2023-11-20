namespace MovementSystem
{
    public class PlayerMovingState : PlayerGroundedState
    {
        public PlayerMovingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        #region IState Methods
        public override void Enter()
        {
            StartAnimation(stateMachine.Player.AnimationData.MovingParameterHash);

            base.Enter();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.MovingParameterHash);
        }
        #endregion
    }
}
