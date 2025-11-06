using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.MinigameFramework.Scripts.Framework.Input {
    public abstract class Pawn : MonoBehaviour {
        /// Do not set this, it will be set by PawnBindingManager
        [HideInInspector] public int playerIndex = -1;

        /// <summary>
        ///     When bound to a PlayerInput by PawnBindingManager, this method will be called when the player presses any button.
        ///     To see which action was pressed, check {context.action.name}
        /// </summary>
        /// <param name="context"></param>
        protected abstract void OnActionPressed(InputAction.CallbackContext context);
        

        /// <summary>
        ///     When bound to a PlayerInput by PawnBindingManager, this method will be called when the player releases any button.
        ///     To see which action was pressed, check {context.action.name}
        /// </summary>
        /// <param name="context"></param>
        protected virtual void OnActionReleased(InputAction.CallbackContext context) { }

        public void HandleActionPressed(InputAction.CallbackContext context) {
            // If WasPerformedThisFrame or is a Value action, execute OnActionPressed
            if (context.action.WasPerformedThisFrame() || context.action.type == InputActionType.Value) OnActionPressed(context);
        }

        public void HandleActionReleased(InputAction.CallbackContext context) {
            // If WasReleasedThisFrame and is not a Value action, execute OnActionReleased
            if (context.action.WasReleasedThisFrame() && context.action.type != InputActionType.Value) OnActionReleased(context);
        }

        private void OnDestroy() {
            // Unbind from PlayerInput when destroyed
            if (playerIndex != -1) PawnBindingManager.UnbindPlayerInputFromPawn(this);
        }
    }
}