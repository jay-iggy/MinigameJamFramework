using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CornMaze {
    public class CornMazePawn : Pawn {
        Vector2 _moveInput = Vector2.zero;
        private Rigidbody2D _rigidbody;
        [SerializeField] public float speed = 8f;

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody2D>();
        }
        void Update() {
            
            _rigidbody.velocity = new Vector2(_moveInput.x * speed, _moveInput.y * speed);
        }

        protected override void OnActionPressed(InputAction.CallbackContext context) {
            if (context.action.name == PawnAction.Move) {
                _moveInput = context.ReadValue<Vector2>();
                
            }

            if (context.action.name == PawnAction.ButtonA) {
                // Jump
                _rigidbody.velocity = new Vector2(_moveInput.x * speed, _moveInput.y * speed);
                //speed++;
            }

            if (context.action.name == PawnAction.ButtonB) {
                // Shoot
                _rigidbody.velocity = new Vector2(_moveInput.x * speed * 2, _moveInput.y * speed *2);
                //this.transform.localScale += new Vector3(1, 1, 1);

            }
        }

        protected override void OnActionReleased(InputAction.CallbackContext context) {
            if (context.action.name == PawnAction.ButtonB) {
                // Stop shooting
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag("Finish"))
            {
                speed += 10;
            }

            Debug.Log("Collision!");
        }
    }
}