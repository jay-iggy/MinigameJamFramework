using System.Collections;
using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Tags;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SnowDay.Icebreaker {
    [RequireComponent(typeof(Rigidbody))]
    public class IcebreakerPawn : Pawn {
        [SerializeField] private float speed = 8f;
        [SerializeField] private float gravity = -50f;

        [SerializeField] private float friction = 0.9f;

        private Vector2 _moveInput = Vector2.zero;

        private Rigidbody _rigidbody;
        private Animator _animator;
        public float rotation = 180;

        private float hammerTime;
        public float hammerCooldown = 0.5f;

        private bool stunned = false;
        public bool alive = true;
        public bool canMove = false;

        // Disable Unity's default gravity when this component is added
        private void Reset() {
            GetComponent<Rigidbody>().useGravity = false;
        }

        private void Awake() {
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponentInChildren<Animator>();
        }

        private void Update() {
            if (!canMove) {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
                return;
            }

            _rigidbody.velocity = (gravity * Time.deltaTime * Vector3.up) +
                                  (_rigidbody.velocity + new Vector3(_moveInput.x * speed * (stunned ? 0.3f : 1), 0,
                                      _moveInput.y * speed * (stunned ? 0.3f : 1))) *
                                  Mathf.Pow(friction, Time.deltaTime + 1);
            if (_rigidbody.velocity.magnitude < 0.7) {
                _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
            }

            transform.eulerAngles = new Vector3(0, Mathf.LerpAngle(transform.eulerAngles.y, rotation, 0.1f), 0);
        }

        // Handle input
        protected override void OnActionPressed(InputAction.CallbackContext context) {
            if (!canMove || !alive) {
                return;
            }

            // Move
            if (context.action.name == "Move") {
                _moveInput = context.ReadValue<Vector2>();
                if (_moveInput.magnitude > 0.1) {
                    rotation = Vector2.Angle(Vector2.up, _moveInput) * (_moveInput.x < 0 ? -1 : 1);
                }
            }

            // Swing Hummer
            if (!stunned && (context.action.name == "ButtonA" || context.action.name == "ButtonB") &&
                Time.time > hammerTime + hammerCooldown) {
                Swing();
                hammerTime = Time.time;
            }
        }

        public void Swing() {
            if (_animator != null) {
                _animator.SetTrigger("Swing");
            }
        }

        public IEnumerator Stun() {
            stunned = true;
            transform.localScale = new Vector3(2, 0.5f, 2);
            GetComponent<SoundEffectController>().Play("Boing");
            yield return new WaitForSeconds(2);
            if (alive) {
                GetComponent<SoundEffectController>().Play("Pop");
            }

            transform.localScale = new Vector3(2, 2, 2);
            stunned = false;
        }

        public bool GetStunned() {
            return stunned;
        }

        public void Confetti() {
            foreach (ParticleSystem p in GameObject.Find("Confetti").GetComponentsInChildren<ParticleSystem>()) {
                p.Play();
            }
        }
    }
}