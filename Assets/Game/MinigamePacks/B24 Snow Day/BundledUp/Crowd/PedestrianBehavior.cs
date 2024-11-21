using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BundledUp {
    public class PedestrianBehavior : MonoBehaviour {
        private enum pedestrianMovment {
            Player,
            Wiggle,
            Circle,
            Dedicated,
            Wander,
            Idle
        }

        [SerializeField] private pedestrianMovment movementType;
        [SerializeField] private List<Color> colorOptions = new();

        private float timer = 0;
        private Vector2 velocity = Vector2.zero;
        private float accelerationMagnitude = 0;
        private Vector2 pivot = Vector2.zero;

        private bool hasEntered = false;
        float entrySpeed;

        public int controlledByIndex = -1;
        [SerializeField] private Transform spriteHolder;
        [SerializeField] private SpriteRenderer[] colorableSprites;
        [SerializeField] private SpriteRenderer highlightSprite;
        private bool highlighted = false;

        void Start() {
            ChooseAttire();
            float entrySpeed = CrowdGameManager.inst.boundaries.x * 1.5f * -Mathf.Sign(transform.position.x) /
                               CrowdGameManager.inst.introDuration;
        }

        public void ChooseMovementType() {
            velocity = Vector2.zero;
            accelerationMagnitude = 0;

            int chosenType = Random.Range(0, 100);
            if (chosenType >= 70) {
                chosenType = (int)pedestrianMovment.Wander;
            }
            else if (chosenType >= 45) {
                chosenType = (int)pedestrianMovment.Circle;
            }
            else if (chosenType >= 25) {
                chosenType = (int)pedestrianMovment.Wiggle;
            }
            else if (chosenType >= 10) {
                chosenType = (int)pedestrianMovment.Dedicated;
            }
            else {
                chosenType = (int)pedestrianMovment.Idle;
            }

            movementType = (pedestrianMovment)chosenType;

            switch (movementType) {
                case pedestrianMovment.Wiggle:
                    WiggleSetUp();
                    break;
                case pedestrianMovment.Circle:
                    CircleSetUp();
                    break;
                case pedestrianMovment.Dedicated:
                    DedicatedSetUp();
                    break;
                case pedestrianMovment.Wander:
                    WanderSetUp();
                    break;
            }
        }

        public void ChooseAttire() {
            spriteHolder.localScale = Vector3.one;
            foreach (SpriteRenderer sr in colorableSprites)
                sr.color = colorOptions[Random.Range(0, colorOptions.Count)];
        }

        public void SetAttire(Color setTo) {
            spriteHolder.localScale = Vector3.one * 2f;
            foreach (SpriteRenderer sr in colorableSprites)
                sr.color = setTo;
        }

        void Update() {
            if (!hasEntered && CrowdGameManager.inst.currentGamePhase == CrowdGameManager.gamePhase.Intro) {
                transform.Translate(Vector3.right * -6.375f * Mathf.Sign(transform.position.x) * Time.deltaTime);
            }
            else if (CrowdGameManager.inst.currentGamePhase == CrowdGameManager.gamePhase.Search) {
                hasEntered = true;

                switch (movementType) {
                    case pedestrianMovment.Wiggle:
                        Wiggle();
                        break;
                    case pedestrianMovment.Circle:
                        Circle();
                        break;
                    case pedestrianMovment.Dedicated:
                        Dedicated();
                        break;
                    case pedestrianMovment.Wander:
                        Wander();
                        break;
                }

                transform.Translate(velocity * Time.deltaTime);
                timer -= Time.deltaTime;

                Vector2 boundaries = CrowdGameManager.inst.boundaries;
                Vector2 center = CrowdGameManager.inst.center;
                transform.position = new Vector2(
                    Mathf.Clamp(transform.position.x - center.x, -boundaries.x, boundaries.x),
                    Mathf.Clamp(transform.position.y - center.y, -boundaries.y, boundaries.y)
                ) + center;
            }
        }

// -----------------------------------------------

        void WiggleSetUp() {
            velocity = Random.insideUnitCircle * Random.Range(1.75f, 2.5f);
        }

        void Wiggle() {
            if (timer <= 0) {
                velocity *= -1;
                timer = Random.Range(.05f, .25f);
            }
        }

// -----------------------------------------------

        void CircleSetUp() {
            float radius = Random.Range(.1f, 1.25f);
            accelerationMagnitude = Random.Range(8f, 16f);
            Vector2 dir = Random.insideUnitCircle * radius;
            pivot = (Vector2)transform.position + dir;
            velocity = Mathf.Sqrt(accelerationMagnitude * radius) * new Vector2(dir.y, -dir.x).normalized *
                       (Random.Range(0, 1) * 2 - 1);
        }

        void Circle() {
            Vector2 acc = (pivot - (Vector2)transform.position).normalized * accelerationMagnitude;
            velocity += acc * Time.deltaTime;
        }

// -----------------------------------------------

        void DedicatedSetUp() {
            velocity = Random.insideUnitCircle * Random.Range(2.75f, 4.25f);
            timer = Random.Range(6f, 14f);
        }

        void Dedicated() {
            if (timer <= 0) {
                DedicatedSetUp();
            }
        }

// -----------------------------------------------

        void WanderSetUp() {
            velocity = Random.insideUnitCircle * Random.Range(2.3f, 3.1f);
            timer = Random.Range(.1f, .9f);
        }

        void Wander() {
            if (timer <= 0) {
                WanderSetUp();
            }
        }

// -----------------------------------------------

        public void SetVelocity(Vector2 setTo) {
            velocity = setTo;
        }

        public bool IsPlayer() {
            return movementType == pedestrianMovment.Player;
        }

        public void MakePlayer(int playerIndex) {
            movementType = pedestrianMovment.Player;
            velocity = Vector2.zero;
            controlledByIndex = playerIndex;
        }

        public void CeasePlayer() {
            controlledByIndex = -1;
            velocity = Vector2.zero;
            ChooseMovementType();
        }

        public void Select(bool setTo) {
            if (setTo || highlighted) {
                spriteHolder.transform.localScale = Vector3.one * 1.5f;
            }
            else {
                spriteHolder.transform.localScale = Vector3.one;
            }
        }

        public void Highlight(Color highlightColor) {
            if (highlightColor == Color.white) { // White is the exit color
                highlighted = false;
                highlightSprite.enabled = false;
                Select(false);
                return;
            }
            else if (!highlighted) {
                highlighted = true;
                highlightSprite.enabled = true;
                highlightSprite.color = highlightColor;
            }
        }

        public bool IsHighlighted() {
            return highlighted;
        }

    }
}