using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Tags;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Examples {
    [RequireComponent(typeof(Rigidbody))]
    public class ExamplePawn : Pawn {
        [SerializeField] private float speed = 8f;
        [SerializeField] private float sprintMultiplier = 1.4f;
        [SerializeField] private float jumpForce = 20f;
        [SerializeField] private float gravity = -50f;

        private bool _isGrounded;

        private float _initialSpeed;

        private Vector2 _moveInput = Vector2.zero;

        private Rigidbody _rigidbody;
        private bool _isSprinting;

        // Disable Unity's default gravity when this component is added
        private void Reset() {
            GetComponent<Rigidbody>().useGravity = false;
        }

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
            _initialSpeed = speed;
        }

        // Handle movement and physics
        private void Update() {
            if (_isSprinting) speed = _initialSpeed * sprintMultiplier;
            else speed = _initialSpeed;
            
            // Gravity
            _rigidbody.velocity += gravity * Time.deltaTime * Vector3.up;
            // Movement
            _rigidbody.velocity = new Vector3(_moveInput.x * speed, _rigidbody.velocity.y, _moveInput.y * speed);
        }

        // Handle grounded state
        private void OnCollisionEnter(Collision other) {
            if (other.collider.HasCustomTag("Ground")) _isGrounded = true;
        }

        // Handle input
        protected override void OnActionPressed(InputAction.CallbackContext context) {
            // Move
            if (context.action.name == "Move") _moveInput = context.ReadValue<Vector2>();

            // Jump
            if (context.action.name == "ButtonA") {
                if (!_isGrounded) return;

                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, jumpForce, _rigidbody.velocity.z);
                _isGrounded = false;
            }

            // Sprint
            if (context.action.name == "ButtonB") _isSprinting = true;
        }

        // Handle input from released buttons
        protected override void OnActionReleased(InputAction.CallbackContext context) {
            // Sprint
            if (context.action.name == "ButtonB") _isSprinting = false;
        }
    }
}