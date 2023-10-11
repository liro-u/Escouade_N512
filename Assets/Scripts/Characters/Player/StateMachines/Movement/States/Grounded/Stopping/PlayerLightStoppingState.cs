using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovementSystem
{
    public class PlayerLightStoppingState : PlayerStoppingState
    {
        private PlayerLightStopData LightStopData;
        public PlayerLightStoppingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            LightStopData = movementData.LightStopData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            stateMachine.ReusableData.MovementDecelerationForce = LightStopData.DecelerationForce;

            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.WeakForce;
        }
        #endregion
    }
}
