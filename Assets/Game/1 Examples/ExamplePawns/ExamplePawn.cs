using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Tags;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Examples {
    [RequireComponent(typeof(Rigidbody))]
    public class ExamplePawn : Pawn {
        [SerializeField] private float speed = 8f;
        [SerializeField] private float jumpForce = 20f;
        [SerializeField] private float gravity = -50f;

        private bool _isGrounded;
        private Vector2 _moveInput = Vector2.zero;
        
        public static bool isPawnInputEnabled = true;

        private Rigidbody _rigidbody;

        // Disable Unity's default gravity when this component is added
        private void Reset() {
            GetComponent<Rigidbody>().useGravity = false;
        }

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
        }

        // Handle movement and physics
        private void Update() { // Gravity
            _rigidbody.linearVelocity += gravity * Time.deltaTime * Vector3.up;
            
            if (!isPawnInputEnabled) {
                _rigidbody.linearVelocity = new Vector3(0, _rigidbody.linearVelocity.y, 0);
                return;
            }
            
            // Movement
            _rigidbody.linearVelocity = new Vector3(_moveInput.x * speed, _rigidbody.linearVelocity.y, _moveInput.y * speed);
        }

        // Handle grounded state
        private void OnCollisionEnter(Collision other) {
            if (other.collider.HasCustomTag("Ground")) _isGrounded = true;
        }

        // Handle input
        protected override void OnActionPressed(InputAction.CallbackContext context) {
            if (!isPawnInputEnabled) return;
            
            // Move
            if (context.action.name == "Move") _moveInput = context.ReadValue<Vector2>();

            // Jump
            if (context.action.name == "ButtonA") {
                if (!_isGrounded) return;

                _rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, jumpForce, _rigidbody.linearVelocity.z);
                _isGrounded = false;
            }
        }
    }
}