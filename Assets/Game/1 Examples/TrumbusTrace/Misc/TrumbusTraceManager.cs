using System;
using System.Collections;
using System.Collections.Generic;
using Game.Examples;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Examples.TrumbusTrace {
    public class TrumbusTraceManager : MonoBehaviour {
        private static WaitForSeconds _waitForSeconds1 = new WaitForSeconds(1f);
        private static WaitForSeconds _waitForSeconds0_5 = new WaitForSeconds(0.5f);

        public static TrumbusTraceManager instance;
        private void Awake() {
            if (instance == null) {
                instance = this;
            }
            else {
                Destroy(gameObject);
            }
            audioSource = GetComponent<AudioSource>();
        }

        [HideInInspector]public List<TraceSubmanager> subscenes = new();
        [SerializeField] GameObject _startText;
        [SerializeField] GameObject _endText;
        [SerializeField] TextMeshProUGUI _timerText;
        [SerializeField] Image _timerBackground;
        [SerializeField] Color _timerWarningColor;

        [SerializeField] private float _duration = 20;
        [SerializeField] private float _warningTime = 5f;
        private AudioSource audioSource;
        [SerializeField] AudioClip tick;
        [SerializeField] AudioClip tock;
        [SerializeField] AudioClip finishWhistle;
        private bool isTick = true;

        void Start() {
            StartCoroutine(StartRoutine());
        }

        IEnumerator StartRoutine() {
            // Disable player inputs during countdown
            ExamplePawn.isPawnInputEnabled = false;
            // Countdown
            yield return _waitForSeconds0_5;
            _startText.SetActive(true);
            audioSource.PlayOneShot(finishWhistle);
            yield return _waitForSeconds1;
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
                
                // Change timer background color right before time runs out
                if (timeLeft <= _warningTime) {
                    if(_timerBackground.color != _timerWarningColor) _timerBackground.color = _timerWarningColor;

                    TickTock();
                    yield return _waitForSeconds0_5;
                    TickTock();
                    yield return _waitForSeconds0_5;
                }
                else {
                    TickTock();
                    yield return _waitForSeconds1;
                }

                timeLeft -= 1f;
                
            }
            _timerText.text = "0";
            StartCoroutine(EndRoutine());
        }

        private void TickTock() {
            //Play sound
            audioSource.PlayOneShot(isTick ? tick : tock);
            isTick = !isTick;
        }

        IEnumerator EndRoutine() {
            // Disable player inputs
            ExamplePawn.isPawnInputEnabled = false;
            // Show end text
            _timerBackground.gameObject.SetActive(false);
            _endText.SetActive(true);
            audioSource.PlayOneShot(finishWhistle);
            yield return new WaitForSeconds(1.5f);
            _endText.SetActive(false);
            // Calculate player scores
            List<int> scores = new();
            foreach (TraceSubmanager subscene in subscenes) {
                scores.Add(subscene.CalculateAndDisplayScore());
            }
            MinigameManager.Ranking ranking = new();
            ranking.DetermineRankingFromScores(scores);
            // Wait to end minigame so players can see their scores
            yield return new WaitForSeconds(6f);
            MinigameManager.instance.EndMinigame(ranking);
            ExamplePawn.isPawnInputEnabled = true;
        }
    }
}