using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MovementSystem
{
    public class PlayerJumpingState : PlayerAirborneState
    {
        private PlayerJumpData jumpData;

        private bool shouldKeepRotating;
        private bool canStartFalling;

        public PlayerJumpingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
            jumpData = airborneData.JumpData;
        }

        #region IState Methods
        public override void Enter()
        {
            base.Enter();

            stateMachine.ReusableData.LastTimePressingJump = null;

            stateMachine.ReusableData.LastTimeLeavingGround = null;

            stateMachine.ReusableData.MovementSpeedModifier = 0f;

            stateMachine.ReusableData.MovementDecelerationForce = jumpData.DecelerationForce;

            stateMachine.ReusableData.VerticalMovementDecelerationForceEnabled = false;

            shouldKeepRotating = stateMachine.ReusableData.MovementInput != Vector2.zero;

            SetRotationData(jumpData.RotationData);

            Jump();
        }

        public override void Exit()
        {
            base.Exit();

            stateMachine.ReusableData.VerticalMovementDecelerationForceEnabled = true;

            SetBaseRotationData();

            canStartFalling = false;
        }

        public override void Update()
        {
            base.Update();

            if (!canStartFalling && IsMovingUp(0f))
            {
                canStartFalling = true;
            }

            if (!canStartFalling || GetPlayerVerticalVelocity().y > 0f)
            {
                return;
            }

            stateMachine.ChangeState(stateMachine.FallingState);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (shouldKeepRotating)
            {
                RotateTowardsTargetRotation();
            }

            if (IsMovingUp())
            {
                DecelerateVertically();
            }
        }
        #endregion

        #region Reusable Methods
        protected override void ResetSprintState()
        {
        }

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            stateMachine.Player.Input.PlayerActions.Jump.canceled += OnJumpCanceled;
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            stateMachine.Player.Input.PlayerActions.Jump.canceled -= OnJumpCanceled;
        }
        #endregion

        #region Main Methods
        private void Jump()
        {
            Vector3 jumpForce = stateMachine.ReusableData.CurrentJumpForce;

            Vector3 jumpDirection = stateMachine.Player.transform.forward;

            if (shouldKeepRotating)
            {
                UpdateTargetRotation(GetMovementInputDirection());

                jumpDirection = GetTargetRotationDirection(stateMachine.ReusableData.CurrentTargetRotation.y);
            }

            jumpForce.x *= jumpDirection.x;
            jumpForce.z *= jumpDirection.z;

            Vector3 capsuleColliderCenterInWorldSpace = stateMachine.Player.ColliderUtility.CapsuleColliderData.Collider.bounds.center;

            Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

            if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, jumpData.JumpToGroundRayDistance, stateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);


                if (IsMovingUp())
                {
                    float horizontalForceModifier = jumpData.HorizontalJumpForceModifierOnSlopeUpwards.Evaluate(groundAngle);
                    float verticalForceModifier = jumpData.VerticalJumpForceModifierOnSlopeUpwards.Evaluate(groundAngle);

                    jumpForce.x *= horizontalForceModifier;
                    jumpForce.y *= verticalForceModifier;
                    jumpForce.z *= horizontalForceModifier;
                }

                if (IsMovingDown())
                {
                    float forceModifier = jumpData.JumpForceModifierOnSlopeDownwards.Evaluate(groundAngle);

                    jumpForce.y *= forceModifier;
                }
            }

            ResetVelocity();

            stateMachine.Player.Rigidbody.AddForce(jumpForce, ForceMode.VelocityChange);
        }
        #endregion

        #region Input Methods
        protected void OnJumpCanceled(InputAction.CallbackContext context)
        {
            stateMachine.ReusableData.VerticalMovementDecelerationForceEnabled = true;
        }
        #endregion
    }
}
