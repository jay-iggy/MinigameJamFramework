using System;
using System.Collections;
using System.Collections.Generic;
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

        public List<TraceSubmanager> subscenes = new();
        [SerializeField] GameObject _startText;
        [SerializeField] GameObject _endText;
        [SerializeField] TextMeshProUGUI _timerText;
        [SerializeField] Image _timerBackground;
        [SerializeField] Color _timerWarningColor;
        private bool _hasStarted = false;

        [SerializeField] private float _duration = 20;
        [SerializeField] private float _warningTime = 5f;

        void Start() {
            StartCoroutine(StartRoutine());
            PlayerManager.onPlayerConnected.AddListener(HandlePlayerJoined);
        }

        IEnumerator StartRoutine() {
            // Disable player inputs during countdown
            foreach (PlayerInput playerInput in PlayerManager.GetConnectedPlayerInputs()) {
                playerInput.currentActionMap.Disable();
            }
            // Countdown
            yield return new WaitForSeconds(0.5f);
            _startText.SetActive(true);
            yield return new WaitForSeconds(1f);
            _startText.SetActive(false);
            // Enable player inputs and start timer
            _hasStarted = true;
            foreach (PlayerInput playerInput in PlayerManager.GetConnectedPlayerInputs()) {
                playerInput.currentActionMap.Enable();
            }
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
            foreach (TraceSubmanager subscene in subscenes) {
                float score = subscene.CalculateAndDisplayScore();
                scores.Add(score);
            }
            // Determine rankings
            MinigameManager.Ranking ranking = new();
            List<int> playerIndices = new();
            for (int i = 0; i < scores.Count; i++) {
                playerIndices.Add(i);
            }
            playerIndices.Sort((a, b) => scores[b].CompareTo(scores[a])); // sort indices by highest score
            for (int rank = 0; rank < playerIndices.Count; rank++) {
                ranking.SetRank(playerIndices[rank], rank+1);
            }
            // Wait to end minigame so players can see their scores
            yield return new WaitForSeconds(6f);
            MinigameManager.instance.EndMinigame(ranking);
        }

        private void HandlePlayerJoined(int playerIndex) {
            // Disable input for newly joined players if the minigame hasn't started
            if (!_hasStarted) {
                PlayerInput playerInput = PlayerManager.players[playerIndex].playerInput;
                playerInput.currentActionMap.Disable();
            }
        }
    }
}