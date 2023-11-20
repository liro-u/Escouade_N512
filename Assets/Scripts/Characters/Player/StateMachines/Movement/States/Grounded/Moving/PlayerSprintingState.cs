using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    public class PlayerSprintingState : PlayerMovingState
    {
        private PlayerSprintData sprintData;

        private bool shouldResetSprintState;

        public PlayerSprintingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            sprintData = movementData.SprintingData;
        }

        #region IState Methods
        public override void Enter()
        {
            stateMachine.ReusableData.MovementSpeedModifier = sprintData.SpeedModifier;

            StartAnimation(stateMachine.Player.AnimationData.SprintParameterHash);

            base.Enter();

            stateMachine.ReusableData.CurrentJumpForce = airborneData.JumpData.StrongForce;

            shouldResetSprintState = true;

            stateMachine.ReusableData.ShouldSprint = true;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(stateMachine.Player.AnimationData.SprintParameterHash);

            if (shouldResetSprintState )
            {
                stateMachine.ReusableData.ShouldSprint = false;
            }
        }
        #endregion

        #region Main Methods
        private void StopSprinting()
        {
            if (stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                stateMachine.ChangeState(stateMachine.HardStoppingState);

                return;
            }

            stateMachine.ChangeState(stateMachine.RunningState);

        }
        #endregion

        #region Reusable Methods
        protected override void OnFall()
        {
            shouldResetSprintState = false;

            base.OnFall();
        }
        #endregion

        #region Input Methods
        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
            shouldResetSprintState = false;

            base.OnJumpStarted(context);
        }

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            stateMachine.ChangeState(stateMachine.HardStoppingState);

            base.OnMovementCanceled(context);
        }
        #endregion
    }
}
