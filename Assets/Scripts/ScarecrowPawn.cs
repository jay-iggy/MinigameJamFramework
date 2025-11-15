using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HotPotatoGame {
    public class ScarecrowPawn : Pawn {
        public float moveSpeed;
        Vector2 _moveInput = Vector2.zero;
        Rigidbody rb;
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();   
        }
        void FixedUpdate() {
            // TODO: Implement movement
            rb.AddForce(new Vector3(_moveInput.x * moveSpeed, 0f, _moveInput.y * moveSpeed));
        }

        protected override void OnActionPressed(InputAction.CallbackContext context) {
            if (context.action.name == PawnAction.Move) {
                _moveInput = context.ReadValue<Vector2>();
            }

            if (context.action.name == PawnAction.ButtonA) {
                // Jump
            }

            if (context.action.name == PawnAction.ButtonB) {
                // Shoot
            }
        }

        protected override void OnActionReleased(InputAction.CallbackContext context) {
            if (context.action.name == PawnAction.ButtonB) {
                // Stop shooting
            }
        }
    }
}