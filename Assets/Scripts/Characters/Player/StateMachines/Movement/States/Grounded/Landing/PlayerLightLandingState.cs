using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MovementSystem
{
    public class PlayerLightLandingState : PlayerLandingState
    {
        protected bool hasJustEnterState = true;
        public PlayerLightLandingState(PlayerMovementStateMachine playerMovementStateMachine) : base(playerMovementStateMachine)
        {
        }

        #region IState Methods
        public override void Enter()
        {
            hasJustEnterState = true;

            stateMachine.ReusableData.MovementSpeedModifier = 0f;

            base.Enter();

            ResetVelocity();
        }

        public override void Update()
        {
            if (hasJustEnterState)
            {
                hasJustEnterState = false;

                return;
            }

            base.Update();

            if (stateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                return;
            }

            OnMove();
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!IsMovingHorizontally())
            {
                return;
            }

            ResetVelocity();
        }

        public override void OnAnimationTransitionEvent()
        {
            stateMachine.ChangeState(stateMachine.IdlingState);
        }
        #endregion
    }
}
