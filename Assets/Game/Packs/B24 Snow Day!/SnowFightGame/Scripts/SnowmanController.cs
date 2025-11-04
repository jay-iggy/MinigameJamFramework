using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.Input;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace SnowDay.SnowFight {
    public class SnowmanController : Pawn {
        // Player Movement
        [SerializeField] private float moveSpeed = 15f;
        [SerializeField] private Rigidbody rb;
        private Vector2 moveInputValue;

        // Camera Rotation
        [SerializeField] private float sensitivityX = 1f;
        [SerializeField] private float sensitivityY = 1f;
        private float rotationX = 0f;
        private float rotationY = 0f;
        private Vector2 lookInputValue;

        // Shooting Snowball
        public Transform firePoint;
        public GameObject snowballPrefab;

        // Properites
        public int health = 100;
        public int lives = 3;
        public TextMeshPro playerText;
        public bool isAlive = true;

        private void Awake() {
            rb = GetComponent<Rigidbody>();
        }

        protected override void OnActionPressed(InputAction.CallbackContext context) {
            Debug.Log(context.action.name);
            switch (context.action.name) {
                case "Move":
                    moveInputValue = context.ReadValue<Vector2>();
                    break;
                case "Look":
                    lookInputValue = context.ReadValue<Vector2>();
                    break;
                case "ButtonR":
                    if (health > 0) {
                        LaunchSnowball();
                    }

                    break;
            }
        }

        private void CameraRotation() {
            rotationX += lookInputValue.x * sensitivityX;
            rotationY += lookInputValue.y * sensitivityY;

            rotationY = Mathf.Clamp(rotationY, -90f, 90f);

            transform.localRotation = Quaternion.Euler(-rotationY, rotationX, 0f);
        }

        private void Move() {
            Vector3 movement = new Vector3(moveInputValue.x, 0, moveInputValue.y);
            rb.velocity = movement * moveSpeed;
        }

        private void LaunchSnowball() {
            GameObject snowball = Instantiate(snowballPrefab, firePoint.position, firePoint.rotation);
            snowball.GetComponent<Snowball>().Shoot(transform.forward);
        }

        IEnumerator Respawn() {
            lives--;
            // Disable player controls and visibility
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            playerText.alpha = 0;

            int randomSpawnX = Random.Range(-15, 18);
            int randomSpawnZ = Random.Range(-25, 11);

            if (lives > 0) {
                yield return new WaitForSeconds(1f);
                transform.position = new Vector3(randomSpawnX, 4, randomSpawnZ);
                health = 100;
                isAlive = true;

                // Re-enable player controls and visibility
                GetComponent<Renderer>().enabled = true;
                GetComponent<Collider>().enabled = true;
                playerText.alpha = 1;
            }
        }

        public void TakeDamage(int damage) {
            health -= damage;
            if (health <= 0) {
                StartCoroutine(Respawn());
            }
        }

        private void Update() {
            CameraRotation();

            if (transform.position.y < 0 && isAlive) {
                isAlive = false;
                TakeDamage(100);
            }
        }

        private void FixedUpdate() {
            Move();
        }

        internal void Die() {
            SnowFightManager snowFightManager = FindObjectOfType<SnowFightManager>();
            snowFightManager.KillPlayer(this);
        }
    }
}