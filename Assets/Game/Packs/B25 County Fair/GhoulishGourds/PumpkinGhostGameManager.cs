using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using TMPro;
using UnityEngine;

namespace PumpkinGhost {
    public class PumpkinGhostGameManager : MonoBehaviour {
        // TIME VARIABLES
        [Header("Time")]
        public float duration = 20;
        private float gameTime = 66.5f;
        [HideInInspector] public float timer = 0;
        // SCORING VARIABLES
        private MinigameManager.Ranking _ranking = new();
        private int[] playerScores = new int[4];
        // UI VARIABLES
        [Header("UI")]
        [SerializeField] private GameObject readyText;

        [SerializeField] private GameObject timerText;
        [SerializeField] private GameObject timerTextDraw;
        [SerializeField] private GameObject timerTextOutline;

        [SerializeField] private GameObject scoreText;
        [SerializeField] private GameObject scoreTextDraw;
        [SerializeField] private GameObject scoreTextOutline;
        
        //SOUND
        [SerializeField] private AudioClip sound_music;
        [SerializeField] private AudioClip sound_intro;
        private AudioSource _audio;

        private int numPlayers;
        

        private void Start() {
            _audio = GetComponent<AudioSource>();
            _ranking.SetAllPlayersToRank(1); // set all players to first place
            StartCoroutine(GameTimer());

            numPlayers = PlayerManager.GetNumPlayers();
        }

        IEnumerator GameTimer() {
            // Intro Animation
            _audio.PlayOneShot(sound_intro);
            PumpkinGhostPawn.isPawnInputEnabled = false;
            //readyText.SetActive(true);
            yield return new WaitForSeconds(1);
            //readyText.SetActive(false);
            PumpkinGhostPawn.isPawnInputEnabled = true;
            _audio.PlayOneShot(sound_music);
            // Timer
            timer = gameTime;
            timerText.SetActive(true);
            scoreText.SetActive(true);
            yield return new WaitForSeconds((int)gameTime);

            StartCoroutine(EndMinigame());
        }

        private void OnTriggerEnter(Collider other) {
            Pawn pawn = other.GetComponent<Pawn>();
            if (pawn != null) {
                KillPlayer(pawn);
            }
        }
        private void KillPlayer(Pawn pawn) {
            /*
            print($"Player {pawn.playerIndex} has been eliminated.");
            
            if(pawn.playerIndex >= 0) { // if pawn is bound to a player
                _ranking[pawn.playerIndex] = 4 - _deaths;
            }
            _deaths++; // also count deaths for pawns not bound to a player

            if (_deaths == 3) {
                StartCoroutine(EndMinigame());
            }
            */
        }

        private void Update()
        {
            _audio.pitch = Time.timeScale;
            if (timerText.activeSelf) 
            {
                timer -= Time.deltaTime;
                timerText.SetActive(true);
                if (timer >= 0) {
                    if (timer > 18) {
                        timerTextDraw.GetComponent<TextMeshProUGUI>().text = ((int) timer).ToString();
                        timerTextOutline.GetComponent<TextMeshProUGUI>().text = ((int) timer).ToString();
                    }
                    else {
                        timerTextDraw.GetComponent<TextMeshProUGUI>().text = (((int) (timer * 2)) / 2f).ToString("F1");
                        timerTextOutline.GetComponent<TextMeshProUGUI>().text = (((int) (timer * 2)) / 2f).ToString("F1");
                    }
                }
                scoreTextDraw.GetComponent<TextMeshProUGUI>().text = "";
                scoreTextOutline.GetComponent<TextMeshProUGUI>().text = "";

                int pnum = 0;
                String colorText = "null";
                foreach (int score in playerScores) {
                    pnum += 1;
                    if (pnum == 1)
                        colorText = "blue";
                    else if (pnum == 2)
                        colorText = "red";
                    else if (pnum == 3)
                        colorText = "green";
                    else
                        colorText = "yellow";
                    
                    if (pnum <= numPlayers) {
                        String textAdd = "P" + (pnum).ToString() + ": " + score;
                        scoreTextDraw.GetComponent<TextMeshProUGUI>().text += "<color=\"" + colorText + "\">" + textAdd + " </color>";
                        scoreTextOutline.GetComponent<TextMeshProUGUI>().text += textAdd + " ";
                    }
                }
            }
        
        }
        
        public void AddScore(int player, float pumpkinSize){
            print(pumpkinSize);
            playerScores[player - 1] += (int) (pumpkinSize * 4.0f) - 3;
            Debug.Log(playerScores[player - 1]);
        }

        private void CalculateScores() {
            _ranking.DetermineRankingFromScores(new List<int>(playerScores));
        }

        IEnumerator EndMinigame() {
            //Calculate Scores
            CalculateScores();
            // End
            PumpkinGhostPawn.isPawnInputEnabled = false;
            yield return new WaitForSeconds(2);
            MinigameManager.instance.EndMinigame(_ranking);
        }
    }
}
