using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {
    [SerializeField] Button startButton;
    public void LoadRandomMinigame() {
        MinigameManager.instance.LoadRandomMinigame();
    }

    private void OnEnable() {
        PlayerManager.onPlayerConnected.AddListener(OnPlayerConnected);
        PlayerManager.onPlayerDisconnected.AddListener(OnPlayerDisconnected);
    }
    private void OnDisable() {
        PlayerManager.onPlayerConnected.RemoveListener(OnPlayerConnected);
        PlayerManager.onPlayerDisconnected.RemoveListener(OnPlayerDisconnected);
    }

    private void Start() {
        PlayerManager.SetMenuActionMap();
    }

    public void Update() {
        startButton.interactable = PlayerManager.AreAllPlayersConnected();
    }
    
    [SerializeField] List<PlayerSlot> playerSlots = new List<PlayerSlot>();
    
    public void OnPlayerConnected(int playerIndex) {
        playerSlots[playerIndex].SetStatus(true);
    }
    public void OnPlayerDisconnected(int playerIndex) {
        playerSlots[playerIndex].SetStatus(false);
    }
    
}
