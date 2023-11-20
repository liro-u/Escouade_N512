using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovementSystem
{
    public class PlayerHardStoppingState : PlayerStoppingState
    {
        private PlayerHardStopData HardStopData;

        public PlayerHardStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            HardStopData = movementData.HardStopData;
        }

        #region IState Methods
        public override void Enter()
        {
            StartAnimation(stateMachine.Player.AnimationData.HardStopParameterHash);

            base.Enter();

            stateMachine.ReusableData.MovementDecelerationForce = HardStopData.DecelerationForce;

            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StrongForce;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.HardStopParameterHash);
        }
        #endregion

        #region Reusable Methods
        protected override void OnMove()
        {
            if (stateMachine.ReusableData.ShouldWalk)
            {
                return;
            }

            stateMachine.ChangeState(stateMachine.RunningState);
        }
        #endregion
    }
}
