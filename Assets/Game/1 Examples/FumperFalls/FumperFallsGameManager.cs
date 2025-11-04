using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework;
using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;

namespace Examples.FumperFalls {
    public class FumperFallsGameManager : MonoBehaviour {
        public float duration = 30;
        public float timer = 0;

        private MinigameManager.Ranking _ranking = new();

        private List<Player> _alivePlayers = new();
        private int _deaths = 0;


        private void Start() {
            _alivePlayers = PlayerManager.players;
            
            StartCoroutine(GameTimer());

            //if not starting in editor (or if all players are bound ahead of time)
            // stop time + player input
            // 3 2 1 countdown
            // start time + player input
        }

        private void OnTriggerEnter(Collider other) {
            Pawn pawn = other.GetComponent<Pawn>();
            if (pawn != null) {
                KillPlayer(pawn);
            }
        }

        private void KillPlayer(Pawn pawn) {
            print($"Player Index: {pawn.playerIndex}");
            
            if(pawn.playerIndex >= 0) {
                _ranking.SetRank(pawn.playerIndex, 4 - _deaths);
                _alivePlayers.Remove(PlayerManager.players[pawn.playerIndex]);
            }
            _deaths++;

            if (_deaths == 3) {
                StopMinigame();
            }
        }

        IEnumerator GameTimer() {
            while (timer < duration) {
                timer += Time.deltaTime;
                yield return null;
            }

            StopMinigame();
        }



        private void StopMinigame() {
            StartCoroutine(EndMinigame());
        }

        IEnumerator EndMinigame() {
            foreach (Player player in _alivePlayers) {
                _ranking.SetRank(player.playerIndex, 1);
            }
            
            // "FINISH" ui
            yield return new WaitForSeconds(2);
            MinigameManager.instance.EndMinigame(_ranking);
        }


    }
}
