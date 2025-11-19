using System;
using System.Diagnostics;
using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using Game.MinigameFramework.Scripts.Tags;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PumpkinGhost {
    [RequireComponent(typeof(Rigidbody))]
    public class PumpkinGhostPawn : Pawn {
        [SerializeField] public int playerNum;

        [SerializeField] private GameObject balloon;
        [SerializeField] private float speed = 0.45f;
        [SerializeField] private float gravity = -80f;
        [SerializeField] private float friction = 0.98f;
        [SerializeField] private AudioClip sound_pumpkinPickup;
        [SerializeField] private AudioClip sound_pumpkinThrow;
        
        private Rigidbody _rigidbody;
        private AudioSource _audio;
        public static bool isPawnInputEnabled = true;
        private Vector2 _moveInput = Vector2.zero;
        public float rotation = 180;

        public GameObject pumpkin;
        public GameObject thrownPumpkin;
        public float pumpkinSize = 0.0f;

        public GameObject pumpkinRespawn;
        public GameObject particles;

        // The pumpkin which this player can pickup (if any)
        public GrowingPumpkin pumpkinPickup = null;

        // Disable Unity's default gravity when this component is added
        private void Reset() {
            GetComponent<Rigidbody>().useGravity = false;
        }
        private void Awake() {
            if (playerNum > 1 && playerNum > PlayerManager.GetNumPlayers()) {
                gameObject.SetActive(false);
                balloon.SetActive(false);
            }

            _rigidbody = GetComponent<Rigidbody>();
            _audio = GetComponent<AudioSource>();

           
        }

        public int get_number(){
            return playerNum;
        }
        
        // Handle input
        protected override void OnActionPressed(InputAction.CallbackContext context) {
            if (!isPawnInputEnabled) return;
            
            // Move
            if (context.action.name == PawnAction.Move) {
                _moveInput = context.ReadValue<Vector2>();
                if (_moveInput.magnitude > 0.1) {
                    rotation = Vector2.Angle(Vector2.up, _moveInput) * (_moveInput.x < 0 ? -1 : 1);
                }
            }

            // A button
            if (context.action.name == PawnAction.ButtonA || context.action.name == PawnAction.ButtonB)
            {
                // Throwing
                if (pumpkinSize > 0)
                {
                    _audio.PlayOneShot(sound_pumpkinThrow);
                    pumpkin.SetActive(false);

                    // Create thrown projectile
                    thrownPumpkin.transform.position = transform.position + ((0.45f + pumpkinSize) * transform.forward);
                    thrownPumpkin.transform.rotation = transform.rotation;
                    thrownPumpkin.transform.localScale = new Vector3(pumpkinSize, pumpkinSize, pumpkinSize);
                    thrownPumpkin.GetComponent<ThrownPumpkin>().playerNum = playerNum;
                    Instantiate(thrownPumpkin);

                    pumpkinSize = 0.0f;
                }
                else
                {
                    // Pickup code
                    if (pumpkinPickup != null)
                    {
                        _audio.PlayOneShot(sound_pumpkinPickup);
                        pumpkinSize = pumpkinPickup.GetSize();
                        pumpkin.SetActive(true);
                        pumpkin.transform.localScale = new Vector3(pumpkinSize * 0.01f, pumpkinSize * 0.01f, pumpkinSize * 0.01f);
                        pumpkin.transform.localPosition = new Vector3(0, 0, 0.45f + (pumpkinSize));
                        pumpkinPickup.Delete();
                        pumpkinPickup = null;
                    }
                }
            }
        }
        
        private void Update() {
            if (!isPawnInputEnabled) {
                _rigidbody.velocity = Vector3.zero;
                _rigidbody.angularVelocity = Vector3.zero;
                return;
            }

            float modifiedSpeed = speed;
            if (pumpkinSize > 0) {
                modifiedSpeed *= 1 / pumpkinSize;
            }
            _rigidbody.velocity = (gravity * Time.deltaTime * Vector3.up) + (((float) Math.Pow(friction, Time.deltaTime + 1)) * (_rigidbody.velocity + new Vector3(_moveInput.x * modifiedSpeed, 0, _moveInput.y * modifiedSpeed)));
            
            if (_rigidbody.velocity.magnitude < 0.7) {
                _rigidbody.velocity = new Vector3(0, _rigidbody.velocity.y, 0);
            }

            transform.eulerAngles = new Vector3(0, Mathf.LerpAngle(transform.eulerAngles.y, rotation, 0.1f), 0);
        }

        public void BreakHeldPumpkin() {
            particles.transform.position = transform.position + ((0.45f + pumpkinSize) * transform.forward);
            Instantiate(particles);

            pumpkinRespawn.transform.position = transform.position + ((0.45f + pumpkinSize) * transform.forward);
            Instantiate(pumpkinRespawn);
            
            pumpkin.SetActive(false);
            pumpkinSize = 0.0f;
        }

        public static explicit operator PumpkinGhostPawn(GameObject v)
        {
            throw new NotImplementedException();
        }

    }
}