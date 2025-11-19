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
        public bool dashing = false;
        public float inactiveTimer = 0;
        Vector2 _moveInput = Vector2.zero;

        Rigidbody rb;
        private float standardDrag;
        private float earlyDragRestorationTime = 0.2f;
        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            standardDrag = rb.drag;
        }

        private void Update()
        {
            if (rb.GetComponent<ObjectBounce>().isShot)
            {
                rb.drag = 1.75f;
                _moveInput = Vector2.zero;
                return;
            }
            else
            {
                rb.drag = standardDrag;
            }
            
            if (inactiveTimer > 0){
                // decrement timer for movement being inactive
                inactiveTimer -= Time.deltaTime;
                if (inactiveTimer < earlyDragRestorationTime)
                {
                    rb.drag = standardDrag;
                }
                else
                {
                    rb.drag = 1;
                }
                return;
            }
            else
            {

                rb.AddForce(new Vector3(0f, -2500f * Time.deltaTime, 0f));
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
        }

        protected override void OnActionPressed(InputAction.CallbackContext context) {

            if (inactiveTimer > 0) return;
            if (rb.GetComponent<ObjectBounce>().isShot) return;

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
                    // Dash
                    GetComponentInChildren<DashBehavior>().Dash(new Vector2(Mathf.Sin(Mathf.Deg2Rad * transform.rotation.eulerAngles.y), Mathf.Cos(Mathf.Deg2Rad * transform.rotation.eulerAngles.y)));
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