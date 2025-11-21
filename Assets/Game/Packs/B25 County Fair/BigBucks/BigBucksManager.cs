using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

namespace CountyFair.BigBucks {
    public class BigBucksManager : MonoBehaviour {

        public static BigBucksManager inst;
        void Awake() {
            inst = this;
        }

        private MinigameManager.Ranking ranking = new();
        private int playersLeft;

        [SerializeField] SpriteRenderer winnerSpotlight;
        [SerializeField] List<BullRiderPawn> playerPawns = new();

        public bool gameEnded = false;

        void Start() {
            playersLeft = PlayerManager.GetNumPlayers();
            if (playersLeft > 0) {
                // sent here from other menus
                for (int i = playersLeft; i < playerPawns.Count; i++) {
                    // silently disable each non player rider
                    playerPawns[i].SilentDisable();
                }
            } else {
                // record num players when first one falls
                playersLeft = -1;
            }

            winnerSpotlight.enabled = false;
        }
        

        public void PlayerFalls(int playerIndex) {
            if (playerIndex == -1) return; // do nothing if non-player falls
            if (playersLeft == -1) playersLeft = PlayerManager.GetNumPlayers();
            Debug.Log(playerIndex + " fell and got " + playersLeft + "st/nd/rd/th place");
            ranking[playerIndex] = playersLeft;
            playersLeft--;
            if (playersLeft <= 1) {
                int winner = 0;
                for (int i = 0; i < PlayerManager.GetNumPlayers(); i++) {
                    if (ranking[i] == 0) {
                        ranking[i] = 1; // set alive to first place
                        winner = i;
                    }
                }
                // turn on spotlight whilee we wait
                winnerSpotlight.transform.position = new Vector3(4.5f * winner, 2, -1);
                winnerSpotlight.enabled = true;
                BBAudio.inst.PlayRandomVictory();
                gameEnded = true;
                Invoke("EndGameAfterDelay", 1.75f);
            }
        }

        private void EndGameAfterDelay() {
            MinigameManager.instance.EndMinigame(ranking);
        }
    }
}
