using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Starter.Basic {
    public class StarterGameManager : MonoBehaviour {
        private MinigameManager.Ranking _ranking = new();

        public void EndMinigame() {
            MinigameManager.instance.EndMinigame(_ranking);
        }
    }
}