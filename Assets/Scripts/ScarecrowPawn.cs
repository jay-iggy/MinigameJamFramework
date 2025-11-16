using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HotPotatoGame { 
    public class ScarecrowPawn : Pawn {
        public Team team;
        public int playerNum;

        public float moveSpeed;
        public float minSpeed;

        public bool holdingPotato = false;
        Vector2 _moveInput = Vector2.zero;

        Rigidbody rb;
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();   
        }

        private void Update()
        {
            rb.AddForce(new Vector3(0f, -1500f * Time.deltaTime, 0f));
            if (_moveInput != Vector2.zero)
            {
                // turn to face direction of motion
                transform.rotation = Quaternion.Euler(new Vector3(0, Mathf.Rad2Deg * Mathf.Atan2(_moveInput.x, _moveInput.y), 0));
            }

            if (_moveInput != Vector2.zero)
            {
                // add force in direction of input - basic movement code
                rb.AddForce(Time.deltaTime * new Vector3(_moveInput.x * moveSpeed, 0f, _moveInput.y * moveSpeed));
            }
            else
            {
                // clamp velocity from 0 to prevent mini-sliding
                if (rb.velocity.magnitude < minSpeed)
                {
                    rb.velocity = rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
                }

            }
        }
        void FixedUpdate() {
            
        }

        protected override void OnActionPressed(InputAction.CallbackContext context) {
            if (context.action.name == PawnAction.Move) {
                _moveInput = context.ReadValue<Vector2>();
            }

            if (context.action.name == PawnAction.ButtonA) {
                if (holdingPotato)
                {
                    // Throw
                    GetComponent<ThrowBehavior>().Throw();
                }
                else
                {
                    // Punch
                    GetComponent<PunchBehavior>().Punch();
                }
            }

            if (context.action.name == PawnAction.ButtonB) {
                // Shoot
            }
        }

        protected override void OnActionReleased(InputAction.CallbackContext context) {
            if (context.action.name == PawnAction.Move)
            {
                _moveInput = Vector2.zero;
            }
            if (context.action.name == PawnAction.ButtonB) {
                // Stop shooting
            }
        }
    }
}