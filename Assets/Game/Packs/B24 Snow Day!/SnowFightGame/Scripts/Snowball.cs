using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SnowDay.SnowFight {
    public class Snowball : MonoBehaviour {
        private Rigidbody rb;
        [SerializeField] private float speedX = 10f;
        [SerializeField] private float speedY = 5f;
        [SerializeField] private GameObject player;

        private void Awake() {
            rb = GetComponent<Rigidbody>();
        }

        public void Shoot(Vector3 playerDirection) {
            rb.AddForce(playerDirection * speedX, ForceMode.Impulse);
            rb.AddForce(Vector3.up * speedY, ForceMode.Impulse);
        }

        private void OnCollisionEnter(Collision other) {
            if (other.gameObject.CompareTag("Player")) {
                SnowmanController playerController = other.gameObject.GetComponent<SnowmanController>();
                if (playerController != null) {
                    playerController.TakeDamage(10);
                    if (playerController.health <= 0 && playerController.lives <= 0) {
                        playerController.Die();
                    }
                }

                Destroy(gameObject);
            }
        }

        private void Update() {
            if (transform.position.y < 2) {
                Destroy(gameObject);
            }
        }
    }
}
