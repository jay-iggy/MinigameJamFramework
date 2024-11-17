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
        private float snowTotal=0;
        [SerializeField] private float distanceToSnow = 0.1f;
        [SerializeField] private AnimationCurve snowSizeCurve;
        [SerializeField] private AnimationCurve snowMassCurve;
        [SerializeField] private AnimationCurve snowSpeedCurve;

        private Vector2 _moveInput = Vector2.zero;

        private Rigidbody _rigidbody;

        // Disable Unity's default gravity when this component is added
        private void Reset() {
            GetComponent<Rigidbody>().useGravity = false;
        }

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
        }
        
        
        public float distanceTraveled = 0;
        private void Update() {
            // Gravity
            _rigidbody.velocity += gravity * Time.deltaTime * Vector3.up;
            // Movement
            _rigidbody.angularVelocity += new Vector3(_moveInput.y * speed, 0, -_moveInput.x * speed);
            
            // if the player's velocity in the same direction of their move input, increase distance traveled
            float dot = Vector3.Dot(_rigidbody.velocity, _moveInput);
            if(dot > 0) {
                distanceTraveled += Vector3.Dot(_rigidbody.velocity, _moveInput) * Time.deltaTime;
                if(distanceTraveled > 0.1f) {
                    IncreaseSnow(distanceTraveled * distanceToSnow);
                    distanceTraveled = 0;
                }
            }
        }

        // Handle input
        protected override void OnActionPressed(InputAction.CallbackContext context) {
            // Move
            if (context.action.name == "Move") _moveInput = context.ReadValue<Vector2>();
        }
        
        private void IncreaseSnow(float snow) {
            SetSnowTotal(snowTotal + snow);
        }
        private void SetSnowTotal(float snow) {
            snowTotal = snow;
            float scale = snowSizeCurve.Evaluate(snowTotal);
            transform.localScale = new Vector3(scale, scale, scale);
            _rigidbody.mass = snowMassCurve.Evaluate(snowTotal);
            speed = snowSpeedCurve.Evaluate(snowTotal);
        }
    }
}