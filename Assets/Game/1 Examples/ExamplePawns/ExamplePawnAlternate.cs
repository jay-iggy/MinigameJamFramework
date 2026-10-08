using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Tags;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Examples {
    [RequireComponent(typeof(Rigidbody))]
    public class ExamplePawnAlternate : PawnAlternate{
        [SerializeField] private float speed = 8f;
        [SerializeField] private float jumpForce = 20f;
        [SerializeField] private float gravity = -50f;

        private bool _isGrounded;

        private Vector2 _moveInput = Vector2.zero;

        private Rigidbody _rigidbody;

        // Disable Unity's default gravity when this component is added
        private void Reset() {
            GetComponent<Rigidbody>().useGravity = false;
        }

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
        }

        // Handle movement and physics
        private void Update() {
            // Gravity
            _rigidbody.linearVelocity += gravity * Time.deltaTime * Vector3.up;
            // Movement
            _rigidbody.linearVelocity = new Vector3(_moveInput.x * speed, _rigidbody.linearVelocity.y, _moveInput.y * speed);
        }

        // Handle grounded state
        private void OnCollisionEnter(Collision other) {
            if (other.collider.HasCustomTag("Ground")) _isGrounded = true;
        }

        #region Handle Input
            protected override void OnMovement(InputAction.CallbackContext context) {
                // Movement
                _moveInput = context.ReadValue<Vector2>();
            }
            protected override void OnButtonA() {
                // Jump
                if (!_isGrounded) return;

                _rigidbody.linearVelocity = new Vector3(_rigidbody.linearVelocity.x, jumpForce, _rigidbody.linearVelocity.z);
                _isGrounded = false;
            }
        #endregion
    }
}