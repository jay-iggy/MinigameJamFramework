using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;

namespace SnowDay.SnowFight {
    public class SnowFightManager : MonoBehaviour {
        MinigameManager.Ranking ranking = new();
        private int deaths = 0;

        private void Start() {
            ranking.SetAllPlayersToRank(1);
        }

        public void KillPlayer(Pawn pawn) {
            print($"Player {pawn.playerIndex} has been eliminated.");
            
            if(pawn.playerIndex >= 0) { // if pawn is bound to a player
                ranking[pawn.playerIndex] = 4 - deaths;
            }
            deaths++; // also count deaths for pawns not bound to a player

            if (deaths == 3 || Debug_AreAllBoundPlayersDead()) {
                StopMinigame();
            }
        }

        private void StopMinigame() {
            StartCoroutine(EndMinigame());
        }

        IEnumerator EndMinigame() {
            yield return new WaitForSeconds(2);
            MinigameManager.instance.EndMinigame(ranking);
        }


        private bool Debug_AreAllBoundPlayersDead() {
            for (int i = 0; i < PlayerManager.GetNumPlayers(); i++) {
                if (ranking[i] == 1) return false;
            }
            return true;
        }
    }
}