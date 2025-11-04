using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework;
using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Examples.FumperFalls {
    public class FumperFallsGameManager : MonoBehaviour {
        // TIME VARIABLES
        public float duration = 30;
        [HideInInspector] public float timer = 0;

        // SCORING VARIABLES
        private MinigameManager.Ranking _ranking = new();
        private int _deaths = 0;

        private void Start() {
            StartCoroutine(GameTimer());
        }
        IEnumerator GameTimer() {
            // TODO: Disable player input
            // TODO: 3 2 1 countdown
            // TODO: Enable player input
            while (timer < duration) {
                timer += Time.deltaTime;
                yield return null;
            }
            StartCoroutine(EndMinigame());
        }

        private void OnTriggerEnter(Collider other) {
            Pawn pawn = other.GetComponent<Pawn>();
            if (pawn != null) {
                KillPlayer(pawn);
            }
        }
        private void KillPlayer(Pawn pawn) {
            print($"Player {pawn.playerIndex} has been eliminated.");
            
            if(pawn.playerIndex >= 0) { // if pawn is bound to a player
                _ranking[pawn.playerIndex] = 4 - _deaths;
            }
            _deaths++; // also count deaths for pawns not bound to a player

            if (_deaths == 3) {
                StartCoroutine(EndMinigame());
            }
        }
        
        IEnumerator EndMinigame() {
            // Set all alive players to first place
            for (int i = 0; i < PlayerManager.GetNumPlayers(); i++) { // for each player we know is bound
                if (_ranking[i] == 0) { // if player's rank is 0, they are alive
                    _ranking[i] = 1;
                }
            }
            
            // TODO: "FINISH" ui
            yield return new WaitForSeconds(2);
            MinigameManager.instance.EndMinigame(_ranking);
        }
    }
}
