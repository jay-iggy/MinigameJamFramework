using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;

/// <summary>
/// Sets the selected UI object to this object for all connected players and when a player connects.
/// </summary>
public class InitialSelection : MonoBehaviour {
    private void Start() {
        PlayerManager.SetSelectedGameObject(gameObject);
        PlayerManager.onPlayerConnected.AddListener(OnPlayerConnected);
    }

    private void OnDestroy() {
        PlayerManager.onPlayerConnected.RemoveListener(OnPlayerConnected);
    }

    private void OnPlayerConnected(int playerIndex) {
        PlayerManager.players[playerIndex].SetSelectedGameObject(gameObject);
    }
}
