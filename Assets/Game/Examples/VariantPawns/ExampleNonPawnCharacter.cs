using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Tags;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Examples {
    /// <summary>
    /// An example character controller that uses the PawnMiddleman rather than inheriting from Pawn.
    /// Subscribe the HandleInput methods to events in the PawnMiddleman.
    /// </summary>
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(PawnMiddleman))]
    public class ExampleNonPawnCharacter : MonoBehaviour {
        [SerializeField] private float speed = 8f;
        [SerializeField] private float sprintMultiplier = 1.4f;
        [SerializeField] private float jumpForce = 20f;
        [SerializeField] private float gravity = -50f;

        private bool _isGrounded;

        private float _initialSpeed;

        private Vector2 _moveInput = Vector2.zero;

        private Rigidbody _rigidbody;
        private bool _isSprinting;

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
            _initialSpeed = speed;
        }

        // Disable Unity's default gravity when this component is added
        private void Reset() {
            GetComponent<Rigidbody>().useGravity = false;
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
        
        //  In the inspector, set these methods to be called by the events in the PawnMiddleman
        #region Handle Input
            public void OnMove(InputAction.CallbackContext context) {
                _moveInput = context.ReadValue<Vector2>();
            }

            public void OnJump(InputAction.CallbackContext context) {
                if (!_isGrounded) return;

                _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, jumpForce, _rigidbody.velocity.z);
                _isGrounded = false;
            }

            public void OnSprint(InputAction.CallbackContext context) {
                _isSprinting = true;
            }

            public void OnSprintReleased(InputAction.CallbackContext context) {
                _isSprinting = false;
            }
        #endregion
        
    }
}