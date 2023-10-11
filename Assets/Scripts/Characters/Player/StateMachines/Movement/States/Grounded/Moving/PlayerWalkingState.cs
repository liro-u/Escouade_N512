using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

namespace MovementSystem
{
    public class PlayerWalkingState : PlayerMovingState
    {
        private PlayerWalkData walkData;
        public PlayerWalkingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            walkData = movementData.WalkData;
        }

        #region IState Methods
        public override void Enter()
        {
            stateMachine.ReusableData.MovementSpeedModifier = movementData.WalkData.SpeedModifier;

            stateMachine.ReusableData.BackwardsCameraRecenteringData = walkData.BackwardsCameraRecenteringData;

            base.Enter();

            StopAnimation(stateMachine.Player.AnimationData.WalkParameterHash);

            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.WeakForce;
        }

        public override void Exit()
        {
            base.Exit();

            StartAnimation(stateMachine.Player.AnimationData.WalkParameterHash);

            SetBaseCameraRecenteringData();
        }
        #endregion

        #region Input Methods
        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);

            stateMachine.ChangeState(stateMachine.RunningState);
        }

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.LightStoppingState);

            base.OnMovementCanceled(context);
        }
        #endregion
    }
}
