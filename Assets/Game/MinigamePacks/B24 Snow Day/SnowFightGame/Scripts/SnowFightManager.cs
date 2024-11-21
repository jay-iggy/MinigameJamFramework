using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;

namespace SnowFight {
    public class SnowFightManager : MonoBehaviour {
        private List<Player> alivePlayers = new();
        MinigameManager.Ranking ranking = new();

        private void Start() {
            alivePlayers.AddRange(PlayerManager.players);
        }

        public void KillPlayer(Pawn pawn) {
            print($"Player Index: {pawn.playerIndex}");
            if (pawn.playerIndex < 0) return;

            Player player = PlayerManager.players[pawn.playerIndex];
            alivePlayers.Remove(player);
            ranking.AddFromEnd(player.playerIndex); // add player to lowest available rank

            if (alivePlayers.Count <= 1) {
                StopMinigame();
            }
        }

        private void StopMinigame() {
            StartCoroutine(EndMinigame());
        }

        IEnumerator EndMinigame() {
            // "FINISH" ui
            foreach (Player player in alivePlayers) {
                ranking.SetRank(player.playerIndex, 1);
            }

            yield return new WaitForSeconds(2);
            MinigameManager.instance.EndMinigame(ranking);
        }
    }
}