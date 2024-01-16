using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    public class PlayerAirborneState : PlayerMovementState
    {
        public PlayerAirborneState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        #region IState Methods
        public override void Enter()
        {
            StartAnimation(stateMachine.Player.AnimationData.AirborneParameterHash);

            base.Enter();

            SetRotationData(airborneData.BaseRotationData);

            ResetSprintState();
        }

        public override void Exit()
        {
            base.Exit();

            SetBaseRotationData();

            StopAnimation(stateMachine.Player.AnimationData.AirborneParameterHash);
        }
        #endregion

        #region Reusable Methods
        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            stateMachine.Player.Input.PlayerActions.Jump.started += OnJumpStarted;
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            stateMachine.Player.Input.PlayerActions.Jump.started -= OnJumpStarted;
        }

        protected override void OnContactWithGround(Collider collider)
        {
            stateMachine.ChangeState(stateMachine.LightLandingState);
        }

        protected virtual void ResetSprintState()
        {
            stateMachine.ReusableData.ShouldSprint = false;
        }
        #endregion

        #region Input Methods
        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            stateMachine.ReusableData.LastTimePressingJump = Time.time;
        }
        #endregion
    }
}
