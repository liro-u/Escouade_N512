using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovementSystem
{
    public class PlayerMediumStoppingState : PlayerStoppingState
    {
        private PlayerMediumStopData MediumStopData;
        public PlayerMediumStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            MediumStopData = movementData.MediumStopData;
        }

        #region IState Methods
        public override void Enter()
        {
            StartAnimation(stateMachine.Player.AnimationData.MediumStopParameterHash);

            base.Enter();

            stateMachine.ReusableData.MovementDecelerationForce = MediumStopData.DecelerationForce;

            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.MediumForce;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.MediumStopParameterHash);
        }
        #endregion
    }
}
