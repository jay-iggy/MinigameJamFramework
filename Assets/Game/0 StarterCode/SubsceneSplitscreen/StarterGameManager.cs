using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Starter.SubsceneSplitscreen {
    /// <summary>
    /// Manages minigame state across separate individual subscenes for each player.
    /// Communicates with each StarterSubmanager.
    /// </summary>
    public class StarterGameManager : MonoBehaviour {
        [HideInInspector]public List<StarterSubmanager> subscenes = new();
        
        public static StarterGameManager instance;
        private void Awake() {
            if (instance == null) {
                instance = this;
            }
            else {
                Destroy(gameObject);
            }
        }

        public void EndMinigame() {
            // Calculate player scores
            List<int> scores = new();
            List<int> playerIndices = new();
            for(int i = 0; i < subscenes.Count; i++) {
                scores.Add(subscenes[i].GetScore());
                playerIndices.Add(i);
            }
            // Sort indices by highest score
            playerIndices.Sort((a, b) => scores[b].CompareTo(scores[a]));
            // Determine rankings (ties not handled in this example)
            MinigameManager.Ranking ranking = new();
            ranking.SetRank(playerIndices.ToArray());
            // End minigame
            MinigameManager.instance.EndMinigame(ranking);
        }
    }
}