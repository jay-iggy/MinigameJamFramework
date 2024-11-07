using System;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.Input;
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
        public static UnityEvent<int> onPlayerConnected = new();
        public static UnityEvent<int> onPlayerDisconnected = new();

        static PlayerManager() {
            // constructor exists to set players list to default value while also having it be get only
            players = new List<Player>();
        }
        
        /// <returns>True if the number of connected PlayerInputs is equal to the max number of players.</returns>
        public static bool AreAllPlayersConnected() {
            return GetConnectedPlayerInputs().Count == maxPlayers; // this should be expected players/readying up rather than checking with max
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
            onPlayerDisconnected.Invoke(playerIndex);
        }

        private static void ReconnectPlayer(int playerIndex, PlayerInput playerInput) {
            players[playerIndex].playerInput = playerInput;
            _disconnectedPlayers.Remove(playerIndex);
            onPlayerConnected.Invoke(playerIndex);
        }
    }
}