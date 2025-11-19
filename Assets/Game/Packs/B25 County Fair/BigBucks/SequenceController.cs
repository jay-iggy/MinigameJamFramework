using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace CountyFair.BigBucks {
    public class SequenceController : MonoBehaviour {

        // Player Binding
        public int playerIndex;
        public UnityEvent playerMissed;
        public BullRiderPawn player;

        // Indicator display
        [Header("Sequence Display")]
        public float indicatorDuration;
        float indicatorSpeed; // set in start
        [SerializeField] GameObject indicatorPrefab;

        // index mappings
        private List<String> indicatorIndices = new() { "ButtonL", "ButtonA", "ButtonB", "ButtonR" };
        [SerializeField] List<Sprite> indicatorSprites = new();
        [SerializeField] Transform start, end;
        private List<Transform> indicators = new();

        // Sequence Interaction
        [Header("Sequence Interaction")]
        public float spawnInterval;
        [SerializeField] float inputWindow;
        [SerializeField] GameObject earlyPopUp, missedPopUp, juicePopUp;
        
        void Start() {
            indicatorSpeed = Vector2.Distance(end.position, start.position) / indicatorDuration;
            Invoke("SpawnIndicatorLoop", spawnInterval);
        }
        
        void SpawnIndicatorLoop() {
            if (player.fallen) return; // stop spawning if the player is dead
            SpawnIndicator();

            spawnInterval *= .985f;
            indicatorDuration *= .985f;
            Invoke("SpawnIndicatorLoop", spawnInterval);
            if (UnityEngine.Random.Range(0, 10) > 5)
                Invoke("SpawnIndicator", spawnInterval * 1.375f);
        }
        
        void SpawnIndicator() {
            String chosenButton = indicatorIndices[UnityEngine.Random.Range(0, indicatorIndices.Count)];
            CreateIndicator(chosenButton);
        }

        void Update() {
            float speed = indicatorSpeed * Time.deltaTime;
            for (int i = 0; i < indicators.Count; i++) {
                Transform ind = indicators[i];
                Vector2 dir = Vector2.down;
                ind.position += (Vector3)(dir * speed);

                // Check for if missed opportunity
                if (end.position.y > ind.position.y) {
                    playerMissed.Invoke();
                    DisplayPopUp("missed", ind.position);
                    indicators.RemoveAt(i);
                    Destroy(ind.gameObject);
                    i--;
                } else if (ind.position.y - inputWindow < end.position.y) {
                    ind.GetComponent<SpriteRenderer>().color = Color.green;
                }
            }
        }

        private void CreateIndicator(String button) {
            int index = indicatorIndices.IndexOf(button);
            Transform i = Instantiate(indicatorPrefab, transform).transform;
            i.position = (Vector2)start.position; // z value of position contains player index
            i.position += Vector3.forward * playerIndex;
            i.position += Vector3.right * (index - 1.5f) * 4.5f / 5f; 
            i.GetComponent<SpriteRenderer>().sprite = indicatorSprites[index];
            i.name = button;

            indicators.Add(i);
        }

        // -1 = error, 1 = success, 0 = ignore
        public int EvaluateInput(String input) {
            if (indicators.Count == 0 || indicators[0].name != input) return 0;

            float distance = Mathf.Abs(end.position.y - indicators[0].position.y);
            float remainingDuration = distance / indicatorSpeed;

            int validity = 0;
            if (distance > inputWindow * 2) {
                validity = 0;
            } else if (distance > inputWindow) {
                DisplayPopUp("early", indicators[0].position);
                PopIndicator();
                validity = -1;
            } else {
                float percision = Mathf.Abs(distance - inputWindow * .5f);
                String message = "Whoopsie, you aren't meant to see this!";
                if (percision < inputWindow / 2f * .2f) {
                    message = "Amazing";
                    validity = 2; // really good, recovers more
                } else if (percision < inputWindow / 2f * .6f) {
                    message = "Nice";
                    validity = 1;
                } else {
                    message = "OK";
                    validity = 1;
                }

                DisplayPopUp(message, indicators[0].position);
                PopIndicator();
                validity = 1;
            }
            return validity;
        }
        
        private void PopIndicator() {
            Destroy(indicators[0].gameObject);
            indicators.RemoveAt(0);
        }

        private void DisplayPopUp(String type, Vector2 where) {
            GameObject prefab = juicePopUp;
            if (type == "early") prefab = earlyPopUp;
            if (type == "missed") prefab = missedPopUp;
            
            GameObject created = Instantiate(prefab, where, Quaternion.identity, transform);

            if (prefab == juicePopUp) {
                created.GetComponent<PopUpBehavior>().SetMessage(type);
            }
        }
    }
}
