using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ShooterMinigame {
    [RequireComponent(typeof(Image))]
    [RequireComponent(typeof(RectTransform))]
    public class ShooterMinigamePlayer : PawnAlternate {
        public event System.Action<int> onScoreUpdated = delegate { };

        [SerializeField]
        float m_shootCooldown = 0.5f;

        [SerializeField]
        float m_moveSpeed = 20;

        [SerializeField]
        BallThrower m_ballThrower;

        RectTransform m_transform;
        Image m_image;

        bool m_canShoot = true;
        Vector2 m_velocity = Vector2.zero;
        public int score { get; private set; }

        public Vector2 Position => m_transform.position;

        public Color Color => m_image.color;

        private void Awake() {
            m_transform = GetComponent<RectTransform>();
            m_image = GetComponent<Image>();
        }

        private void Update() {
            RectTransform parent = m_transform.parent as RectTransform;
            Vector2 dx = m_velocity * Time.deltaTime * parent.localScale;
            m_transform.position += new Vector3(dx.x, dx.y);
            m_transform.position = new Vector3(
                Mathf.Clamp(m_transform.position.x, 0, parent.sizeDelta.x * parent.localScale.x),
                Mathf.Clamp(m_transform.position.y, 0, parent.sizeDelta.y * parent.localScale.y)
            );
        }

        protected override void OnMovement(InputAction.CallbackContext context) {
            Vector2 input = context.ReadValue<Vector2>();
            m_velocity = input * m_moveSpeed;
        }

        protected override void OnButtonA() {
            Shoot();
        }

        protected override void OnButtonB() {
            Shoot();
        }

        private void Shoot() {
            if (this != null && m_canShoot && TargetPracticeManager.CanShoot) {
                m_ballThrower.ThrowBall(this);
                StartCoroutine(ShootDelay());
            }
        }

        private IEnumerator ShootDelay() {
            m_canShoot = false;
            yield return new WaitForSeconds(m_shootCooldown);
            m_canShoot = true;
        }

        public void Score() {
            score++;
            onScoreUpdated(score);
        }
    }
}