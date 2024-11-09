using System;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework;
using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;

namespace Game.Examples {
    public class PawnManager : MonoBehaviour {
        // This script binds pawns to player inputs
        
        [SerializeField] private List<Pawn> pawns;
        
        private void Start() {
            PlayerManager.SetMinigameActionMap();
            BindPawns();
            
            PlayerManager.onPlayerConnected.AddListener(BindPawn);
            
            
            PlayerManager.onPlayerDisconnected.AddListener(OnPlayerLeft);
        }

        private void BindPawns() {
            foreach(Player player in PlayerManager.players) {
                int index = player.playerIndex;
                if(index < pawns.Count) {
                    PawnBindingManager.BindPlayerInputToPawn(index, pawns[index]);
                }
            }
        }
        
        private void BindPawn(int playerIndex) {
            if (AreAllPawnsBound()) {
                Debug.LogWarning($"PawnManager: All pawns are already bound. Player index {playerIndex} was not bound.");
                return;
            }
            
            if(pawns.Count <= playerIndex) {
                Debug.LogError($"PawnManager: Pawn missing for player index {playerIndex}");
                return;
            }
            PawnBindingManager.BindPlayerInputToPawn(playerIndex, pawns[playerIndex]);
        }
        
        private void OnDisable() {
            PlayerJoinManager.onAllPlayersJoined.RemoveListener(BindPawns); // stop listening when this component is disabled
        }

        private void OnPlayerLeft(int playerIndex) {
            PawnBindingManager.UnbindPlayerInputFromPawn(pawns[playerIndex]);
            PlayerManager.onPlayerConnected.AddListener(BindPawn);
        }
        
        private bool AreAllPawnsBound() {
            return pawns.TrueForAll(pawn => pawn.playerIndex >= 0);
        }
    }
}