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
        public float duration = 20;
        [HideInInspector] public float timer = 0;

        // SCORING VARIABLES
        private MinigameManager.Ranking _ranking = new();
        private int _deaths = 0;

        private void Start() {
            _ranking.SetAllPlayersToRank(1); // set all players to first place
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
            // TODO: "FINISH" ui
            yield return new WaitForSeconds(2);
            MinigameManager.instance.EndMinigame(_ranking);
        }
    }
}
