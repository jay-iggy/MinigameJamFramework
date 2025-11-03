using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;

public class InitialSelection : MonoBehaviour {
    private void Start() {
        PlayerManager.SetSelectedGameObject(gameObject);
        PlayerManager.onPlayerConnected.AddListener(onPlayerConnected);
    }

    private void OnDestroy() {
        PlayerManager.onPlayerConnected.RemoveListener(onPlayerConnected);
    }

    private void onPlayerConnected(int playerIndex) {
        PlayerManager.players[playerIndex].SetSelectedGameObject(gameObject);
    }
}
