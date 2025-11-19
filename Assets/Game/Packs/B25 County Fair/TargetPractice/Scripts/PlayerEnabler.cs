using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterMinigame {
    public class PlayerEnabler : MonoBehaviour {

        [SerializeField]
        int m_playerIndex = 0;

        private void Awake() {
            PlayerManager.onPlayerConnected.AddListener(OnPlayerJoin);
            PlayerManager.onPlayerDisconnected.AddListener(OnPlayerLeave);
            UpdateEnabled();
        }

        private void OnDestroy() {
            PlayerManager.onPlayerConnected.RemoveListener(OnPlayerJoin);
            PlayerManager.onPlayerDisconnected.RemoveListener(OnPlayerLeave);
        }

        private void OnPlayerJoin(int id) {
            UpdateEnabled();
        }

        private void OnPlayerLeave(int id) {
            UpdateEnabled();
        }

        private void UpdateEnabled() {
            gameObject.SetActive(PlayerManager.players.Count > m_playerIndex);
        }
    }
}