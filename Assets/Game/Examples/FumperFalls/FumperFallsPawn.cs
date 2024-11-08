using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Tags;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Examples {
    [RequireComponent(typeof(Rigidbody))]
    public class FumperFallsPawn : Pawn {
        [SerializeField] private float speed = 8f;
        [SerializeField] private float gravity = -50f;

        private Vector2 _moveInput = Vector2.zero;

        private Rigidbody _rigidbody;

        // Disable Unity's default gravity when this component is added
        private void Reset() {
            GetComponent<Rigidbody>().useGravity = false;
        }

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        private void Update() {
            // Gravity
            _rigidbody.velocity += gravity * Time.deltaTime * Vector3.up;
            // Movement
            _rigidbody.angularVelocity += new Vector3(_moveInput.y * speed, 0, -_moveInput.x * speed);
        }

        // Handle input
        protected override void OnActionPressed(InputAction.CallbackContext context) {
            // Move
            if (context.action.name == "Move") _moveInput = context.ReadValue<Vector2>();
        }
    }
}