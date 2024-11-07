using System;
using System.Collections.Generic;
using System.Linq;
using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Framework.Minigames;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine.InputSystem;

namespace Game.MinigameFramework.Scripts.Framework {
    public static class PawnBindingManager {
        /// <summary>
        /// List of pawns that are currently bound to PlayerInputs. When unbinding pawns, iterate through this list and call UnbindPlayerInputFromPawn on each pawn.
        /// </summary>
        private static List<Pawn> _boundPawns = new();
        

        #region Bind and Unbind PlayerInputs to Pawns
        /// <summary>
        /// Bind a pawn to a PlayerInput. This will bind the pawn's OnActionPressed and OnActionReleased methods to the PlayerInput's action map.
        /// </summary>
        /// <param name="playerIndex">The index of the player to bind to this pawn.</param>
        /// <param name="pawn">The pawn to bind. </param>
        public static void BindPlayerInputToPawn(int playerIndex, Pawn pawn) {
            if (playerIndex < 0 || playerIndex >= PlayerManager.players.Count) {
                string message = $"Failed to bind pawn {pawn.name}, player index {playerIndex} is out of range (0-{PlayerManager.players.Count}).";
                throw new Exception(message);
            }
            
            PlayerInput playerInput = PlayerManager.players[playerIndex].playerInput;

            playerInput.currentActionMap.actionTriggered += pawn.HandleActionPressed;
            playerInput.currentActionMap.actionTriggered += pawn.HandleActionReleased;
            
            // Add to list of bound pawns so that it can later be unbound
            pawn.playerIndex = playerIndex;
            _boundPawns.Add(pawn);
        }

        /// <summary>
        /// Unbind a pawn from its current PlayerInput.
        /// If the pawn is not bound to a PlayerInput, this will throw an exception.
        /// </summary>
        /// <param name="pawn"></param>
        /// <exception cref="Exception"></exception>
        public static void UnbindPlayerInputFromPawn(Pawn pawn) {
            if (pawn.playerIndex < 0 || pawn.playerIndex >= PlayerManager.players.Count) {
                string message = $"Failed to unbind pawn {pawn.name}, player index {pawn.playerIndex} is out of range (0-{PlayerManager.players.Count}).";
                throw new Exception(message);
            }

            PlayerInput playerInput = PlayerManager.players[pawn.playerIndex].playerInput;

            if (playerInput != null) {
                playerInput.currentActionMap.actionTriggered -= pawn.HandleActionPressed;
                playerInput.currentActionMap.actionTriggered -= pawn.HandleActionReleased;
            }

            // Remove pawn from list of bound pawns and remove its playerIndex
            pawn.playerIndex = -1;
            _boundPawns.Remove(pawn);
        }
        
        public static void UnBindAllPawns() {
            foreach (Pawn boundPawn in _boundPawns.ToList()) {
                UnbindPlayerInputFromPawn(boundPawn);
            }
        }
        #endregion
    }
}