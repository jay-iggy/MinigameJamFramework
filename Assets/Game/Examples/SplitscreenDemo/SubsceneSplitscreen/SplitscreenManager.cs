using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;
using UnityEngine.Serialization;

namespace Examples.Splitscreen {
    public class SplitscreenManager : MonoBehaviour {
        [SerializeField] private SubsceneManager subscenePrefab;
        [HideInInspector] public List<SubsceneManager> loadedSubscenes;

        // Define splitscreen camera viewports
        public Rect[] cameraRect =
        { new Rect(0, 0.5f,0.5f,0.5f), new Rect(0.5f, 0.5f,0.5f,0.5f), new Rect(0, 0,0.5f,0.5f), new Rect(0.5f, 0,0.5f,0.5f) };
        
        // Singleton instance allows subscenes to access this manager
        public static SplitscreenManager instance;
        private void Awake() {
            if (instance == null) {
                instance = this;
            }
            else {
                Destroy(gameObject);
            }
        }

        private void Start() {
            PlayerManager.SetMinigameActionMap();
            
            // Instantiate subscenes for already connected players
            for (int i = 0; i < PlayerManager.players.Count; i++) {
                Instantiate(subscenePrefab);
            }
            // Listen for new player connections
            PlayerManager.onPlayerConnected.AddListener(OnPlayerConnected);
        }

        private void OnPlayerConnected(int playerIndex) {
            // Create new subscene for newly connected player
            if (playerIndex == loadedSubscenes.Count) {
                Instantiate(subscenePrefab);
            }
            // Reconnect player to existing subscene
            else if (playerIndex < loadedSubscenes.Count && loadedSubscenes[playerIndex] != null) {
                PawnBindingManager.BindPlayerInputToPawn(playerIndex, loadedSubscenes[playerIndex].pawn);
            }
        }
    }
}
