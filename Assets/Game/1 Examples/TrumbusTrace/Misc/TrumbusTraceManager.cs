using System;
using System.Collections;
using System.Collections.Generic;
using Game.Examples;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Examples.TrumbusTrace {
    public class TrumbusTraceManager : MonoBehaviour {
        public static TrumbusTraceManager instance;
        private void Awake() {
            if (instance == null) {
                instance = this;
            }
            else {
                Destroy(gameObject);
            }
        }

        [HideInInspector]public List<TraceSubmanager> subscenes = new();
        [SerializeField] GameObject _startText;
        [SerializeField] GameObject _endText;
        [SerializeField] TextMeshProUGUI _timerText;
        [SerializeField] Image _timerBackground;
        [SerializeField] Color _timerWarningColor;

        [SerializeField] private float _duration = 20;
        [SerializeField] private float _warningTime = 5f;

        void Start() {
            StartCoroutine(StartRoutine());
        }

        IEnumerator StartRoutine() {
            // Disable player inputs during countdown
            ExamplePawn.isPawnInputEnabled = false;
            // Countdown
            yield return new WaitForSeconds(0.5f);
            _startText.SetActive(true);
            yield return new WaitForSeconds(1f);
            _startText.SetActive(false);
            // Enable player inputs and start timer
            ExamplePawn.isPawnInputEnabled = true;
            StartCoroutine(TimerRoutine());
        }

        IEnumerator TimerRoutine() {
            float timeLeft = _duration;
            while (timeLeft > 0) {
                // Update timer text
                _timerText.text = Mathf.CeilToInt(timeLeft).ToString();
                yield return new WaitForSeconds(1f);
                timeLeft -= 1f;
                // Change timer background color right before time runs out
                if (timeLeft <= _warningTime && _timerBackground.color != _timerWarningColor) {
                    _timerBackground.color = _timerWarningColor;
                }
            }
            _timerText.text = "0";
            StartCoroutine(EndRoutine());
        }

        IEnumerator EndRoutine() {
            // Disable player inputs
            foreach (PlayerInput playerInput in PlayerManager.GetConnectedPlayerInputs()) {
                playerInput.currentActionMap.Disable();
            }
            // Show end text
            _timerBackground.gameObject.SetActive(false);
            _endText.SetActive(true);
            yield return new WaitForSeconds(1.5f);
            _endText.SetActive(false);
            // Calculate player scores
            List<float> scores = new();
            List<int> playerIndexList = new();
            for(int i = 0; i < subscenes.Count; i++) {
                scores.Add(subscenes[i].CalculateAndDisplayScore());
                playerIndexList.Add(i);
            }
            // Sort indices by highest score
            playerIndexList.Sort((a, b) => scores[b].CompareTo(scores[a])); 
            // Determine rankings (ties not handled in this example)
            MinigameManager.Ranking ranking = new();
            ranking.SetRanksFromPlayerIndexList(playerIndexList.ToArray());
            // Wait to end minigame so players can see their scores
            yield return new WaitForSeconds(6f);
            MinigameManager.instance.EndMinigame(ranking);
        }
    }
}