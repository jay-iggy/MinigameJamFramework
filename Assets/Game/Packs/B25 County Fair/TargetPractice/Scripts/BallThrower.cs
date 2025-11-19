using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterMinigame {
    [RequireComponent(typeof(Camera))]
    public class BallThrower : MonoBehaviour {

        [SerializeField]
        private GameObject m_ballPrefab;

        [SerializeField]
        private float m_throwSpeed;

        [SerializeField]
        private float m_throwOffset;

        private Camera m_camera;

        private void Awake() {
            m_camera = GetComponent<Camera>();
        }

        public void ThrowBall(ShooterMinigamePlayer owner) {
            GameObject newBall = Instantiate(m_ballPrefab);
            Rigidbody rb = newBall.GetComponent<Rigidbody>();
            Ray throwRay = m_camera.ScreenPointToRay(owner.Position);

            rb.velocity = throwRay.direction * m_throwSpeed;
            newBall.transform.position = throwRay.origin + throwRay.direction * m_throwOffset;

            Ball ball = newBall.GetComponent<Ball>();
            ball.Owner = owner;
        }
    }
}