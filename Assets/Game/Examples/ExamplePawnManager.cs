using System;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework;
using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;

namespace Game.Examples {
    public class ExamplePawnManager : MonoBehaviour {
        // This script binds pawns to player inputs
        // This may not be ideal for all use cases, and you may want it to modify it to fit your game's needs or your development style
        // You need to bind pawns to player inputs, but you don't necessarily have to use this script to do it
        
        // All pawns are bound at once and if a player leaves, all pawns are unbound and then rebound when the player rejoins
        // The list of pawns is also required to be set in the inspector and the number of pawns must match the number of connected players
        // So debugging with just one player might be a little more tedious
        
        [SerializeField] private List<Pawn> pawns;
        
        private void Start() {
            PlayerManager.SetMinigameActionMap();
            if (PlayerManager.AreAllPlayersConnected()) {  
                BindPawns();
            }
            else { 
                // wait to bind pawns until all players are connected
                PlayerJoinManager.onAllPlayersJoined.AddListener(BindPawns);
                Time.timeScale = 0;
            }
            
            PlayerJoinManager.onPlayerLeft.AddListener(OnPlayerLeft);
        }
        
        private void BindPawns() {
            if (pawns.Count <= 0) return; // prevent errors when no pawns are set
            if (pawns.Count != PlayerManager.GetConnectedPlayerInputs().Count) {
                string message = $"Number of pawns ({pawns.Count}) does not match number of connected players ({PlayerManager.GetConnectedPlayerInputs().Count})";
                Debug.LogError(message);
            }
            
            for (int i = 0; i < pawns.Count; i++) {
                if (i < PlayerManager.players.Count) {
                    PawnBindingManager.BindPlayerInputToPawn(i, pawns[i]);
                }
            }
            
            PlayerJoinManager.onAllPlayersJoined.RemoveListener(BindPawns); // stop listening because pawns are now bound
            Time.timeScale = 1;
        }
        
        private void OnDisable() {
            PlayerJoinManager.onAllPlayersJoined.RemoveListener(BindPawns); // stop listening when this component is disabled
        }

        private void OnPlayerLeft() {
            PawnBindingManager.UnBindAllPawns();
            PlayerJoinManager.onAllPlayersJoined.AddListener(BindPawns);
            Time.timeScale = 0;
        }
    }
}