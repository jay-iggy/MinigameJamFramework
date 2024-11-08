using System;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.Input;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game.MinigameFramework.Scripts.Framework.PlayerInfo {
    /// <summary>
    /// Maintains list of players. Can connect and disconnect players. Can get a list of PlayerInputs for all players.
    /// </summary>
    public static class PlayerManager {
        public static List<Player> players { get; }
        private static List<int> _disconnectedPlayers = new();
        public static int maxPlayers = 4;
        public static int expectedPlayers = 4;
        public static UnityEvent<int> onPlayerConnected = new();
        public static UnityEvent<int> onPlayerDisconnected = new();
        private static string _currentActionMap = "Menu";

        static PlayerManager() {
            // constructor exists to set players list to default value while also having it be get only
            players = new List<Player>();
        }
        
        /// <returns>True if the number of connected PlayerInputs is equal to the max number of players.</returns>
        public static bool AreAllPlayersConnected() {
            return GetConnectedPlayerInputs().Count == expectedPlayers;
        }

        /// <returns>List of all connected PlayerInputs</returns>
        public static List<PlayerInput> GetConnectedPlayerInputs() {
            List<PlayerInput> playerInputs = new();
            foreach (Player player in players) {
                if (player.playerInput != null) {
                    playerInputs.Add(player.playerInput);
                }
            }
            return playerInputs;
        }

        /// <summary>
        /// Add a new player to the list of players. Will throw an exception if the max number of players has been reached.
        /// </summary>
        /// <param name="playerInput">The PlayerInput to connect to the newly created player.</param>
        /// <exception cref="Exception">Failed to add new player.</exception>
        public static void ConnectPlayer(PlayerInput playerInput) {
            playerInput.SwitchCurrentActionMap(_currentActionMap);
            
            if (_disconnectedPlayers.Count > 0) {
                ReconnectPlayer(_disconnectedPlayers[0], playerInput);
                return;
            }
            if (players.Count >= maxPlayers) {
                throw new Exception("Failed to add new player. Max players reached.");
            }

            // Initialize new player of the type specified during construction of this PlayerManager
            Player newPlayer = new();
            newPlayer.playerIndex = players.Count;
            newPlayer.playerInput = playerInput;
            players.Add(newPlayer);
            onPlayerConnected.Invoke(newPlayer.playerIndex);
        }

        /// <summary>
        ///     Disconnect the PlayerInput from its connected player. Will allow another PlayerInput to reconnect to that player.
        /// </summary>
        /// <param name="playerInput">PlayerInput to disconnect.</param>
        public static void DisconnectPlayer(PlayerInput playerInput) {
            int playerIndex = players.FindIndex(player => player.playerInput == playerInput);
            players[playerIndex].playerInput = null;
            _disconnectedPlayers.Add(playerIndex);
            _disconnectedPlayers.Sort();
            onPlayerDisconnected.Invoke(playerIndex);
        }

        private static void ReconnectPlayer(int playerIndex, PlayerInput playerInput) {
            players[playerIndex].playerInput = playerInput;
            _disconnectedPlayers.Remove(playerIndex);
            onPlayerConnected.Invoke(playerIndex);
        }

        public static void SetMenuActionMap() {
            _currentActionMap = "Menu";
            foreach (Player player in players) {
                player.playerInput.SwitchCurrentActionMap("Menu");
            }
        }

        public static void SetMinigameActionMap() {
            _currentActionMap = "Minigame";
            foreach (Player player in players) {
                player.playerInput.SwitchCurrentActionMap("Minigame");
            }
        }
    }
}