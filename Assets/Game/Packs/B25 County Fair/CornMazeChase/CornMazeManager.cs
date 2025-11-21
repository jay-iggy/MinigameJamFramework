using System;
using System.Collections;
using System.Diagnostics;
using Game.Examples;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CornMaze {
    public class CornMazeManager : MonoBehaviour {
        public static CornMazeManager instance;
        private void Awake() {
            if (instance == null) {
                instance = this;
            }
            else {
                Destroy(gameObject);
            }
        }
        private MinigameManager.Ranking _ranking = new();
        [SerializeField] TextMeshProUGUI _startTextText;
        [SerializeField] TextMeshProUGUI _startTextShadow;
        [SerializeField] TextMeshProUGUI _endTextText;
        [SerializeField] TextMeshProUGUI _endTextShadow;
        [SerializeField] GameObject _startText;
        [SerializeField] GameObject _endText;
        [SerializeField] TextMeshProUGUI _timerText;
        [SerializeField] Image _timerBackground;
        [SerializeField] Color _timerWarningColor;
        [SerializeField] GameObject[] _pawns;

        [SerializeField] private float _duration = 30;
        [SerializeField] private float _warningTime = 5f;
        [SerializeField] private int runnerIndex;
        [SerializeField] private int[] scores;

        void Start()
        {
            runnerIndex = Random.Range(0, PlayerManager.GetNumPlayers()-1);
            _pawns[runnerIndex].transform.position = new Vector3(0f,-39.5999985f,92.3940964f);
            _pawns[runnerIndex].GetComponent<CornMazePawn>().speed += 10;
            StartCoroutine(StartRoutine());
            _startTextText.text = "Block player "+(runnerIndex + 1);
            _startTextShadow.text = "Block player "+(runnerIndex + 1);

        }

        private void Update()
        {
            if (_pawns[runnerIndex].transform.position.y > 64)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (i != runnerIndex)
                    {
                        _ranking[i] = 4;
                    }
                    else
                    {
                        _ranking[i] = 1;
                    }
                }
                _endTextText.text = "Runner Wins";
                _endTextShadow.text = "Runner Wins";
                StopCoroutine(TimerRoutine());
                StartCoroutine(EndRoutine());
            }
        }

        IEnumerator StartRoutine() {
            // Disable player inputs during countdown
            ExamplePawn.isPawnInputEnabled = false;
            // Countdown
            yield return new WaitForSeconds(0.5f);
            _startText.SetActive(true);
            yield return new WaitForSeconds(2f);
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

            for (int i = 0; i < 4; i++)
            {
                if (i != runnerIndex)
                {
                    _ranking[i] = 2;
                }
                else
                {
                    _ranking[i] = 4;
                }
            }
            
            _endTextText.text = "Blockers Win";
            _endTextShadow.text = "Blockers Win";
            _timerText.text = "0";
            StartCoroutine(EndRoutine());
        }
        IEnumerator EndRoutine() {
            // Disable player inputs
            ExamplePawn.isPawnInputEnabled = false;
            // Show end text
            _timerBackground.gameObject.SetActive(false);
            _endText.SetActive(true);
            yield return new WaitForSeconds(3f);
            _endText.SetActive(false);
            // Calculate player scores

            // Wait to end minigame so players can see their scores
            MinigameManager.instance.EndMinigame(_ranking);
            ExamplePawn.isPawnInputEnabled = true;
        }

    }
}