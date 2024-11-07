using System;
using Game.MinigameFramework.Scripts.Framework;
using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;

namespace Game.Examples {
    public class ExamplePawnManager : MonoBehaviour {
        // This script binds pawns to player inputs
        // This may not be ideal for all use cases, and you may want it to modify it to fit your game's needs or your development style
        
        // All pawns are bound at once and if a player leaves, all pawns are unbound and then rebound when the player rejoins
        // The list of pawns is also required to be set in the inspector and the number of pawns must match the number of connected players
        // So debugging with just one player might be a little more tedious
        
        [SerializeField] private Pawn[] pawns;
        
        private void Start() {
            if (PlayerManager.AreAllPlayersConnected()) {  
                BindPawns();
            }
            else { 
                // wait to bind pawns until all players are connected
                PlayerJoinManager.onAllPlayersJoined.AddListener(BindPawns);
            }
            
            PlayerJoinManager.onPlayerLeft.AddListener(OnPlayerLeft);
        }
        
        private void BindPawns() {
            if (pawns.Length <= 0) return; // prevent errors when no pawns are set
            if (pawns.Length != PlayerManager.GetConnectedPlayerInputs().Count) return; // prevent errors when the number of pawns doesn't match the number of player inputs
            
            for (int i = 0; i < pawns.Length; i++) {
                if (i < PlayerManager.players.Count) {
                    PawnBindingManager.BindPlayerInputToPawn(i, pawns[i]);
                }
            }
            
            PlayerJoinManager.onAllPlayersJoined.RemoveListener(BindPawns); // stop listening because pawns are now bound
        }
        
        private void OnDisable() {
            PlayerJoinManager.onAllPlayersJoined.RemoveListener(BindPawns); // stop listening when this component is disabled
        }

        private void OnPlayerLeft() {
            PawnBindingManager.UnBindAllPawns();
            PlayerJoinManager.onAllPlayersJoined.AddListener(BindPawns);
        }
    }
}