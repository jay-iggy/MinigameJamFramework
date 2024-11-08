using System;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game.MinigameFramework.Scripts.Framework.Input {
    /// <summary>
    /// Handles PlayerInputManager events and connects/disconnects players to the PawnBindingManager 
    /// </summary>
    [RequireComponent(typeof(PlayerInputManager))]
    public class PlayerJoinManager : MonoBehaviour {
        public static UnityEvent onAllPlayersJoined = new();
        public static UnityEvent onPlayerLeft = new();
        private PlayerInputManager _playerInputManager;

        [HideInInspector] public PlayerJoinManager instance;
        
        private void Awake() {
            if (instance == null) {
                instance = this;
            }
            else {
                Destroy(gameObject);
            }

            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            _playerInputManager = GetComponent<PlayerInputManager>();
            
            PlayerManager.expectedPlayers = _playerInputManager.maxPlayerCount;
        }

        private void OnEnable() {
            _playerInputManager.onPlayerJoined += OnPlayerJoined;
            _playerInputManager.onPlayerLeft += OnPlayerLeft;
        }

        private void OnDisable() {
            if (_playerInputManager == null) {
                // If the PlayerJoinManager is destroyed, it will still try to unsubscribe from events on no longer existing component
                return;
            }
            _playerInputManager.onPlayerJoined -= OnPlayerJoined;
            _playerInputManager.onPlayerLeft -= OnPlayerLeft;
        }

        public void OnPlayerJoined(PlayerInput playerInput) {
            try {
                PlayerManager.ConnectPlayer(playerInput);
                DontDestroyOnLoad(playerInput.gameObject);

                if (PlayerManager.AreAllPlayersConnected()) {
                    onAllPlayersJoined.Invoke();
                }

                playerInput.onDeviceLost += OnPlayerLeft;

            }
            catch (Exception maxPlayersException) {
                Debug.Log(maxPlayersException);
            }
        }

        public void OnPlayerLeft(PlayerInput playerInput) {
            onPlayerLeft.Invoke();
            PlayerManager.DisconnectPlayer(playerInput);

            playerInput.onDeviceLost -= OnPlayerLeft;
            
            Destroy(playerInput.gameObject);
        }
    }
}