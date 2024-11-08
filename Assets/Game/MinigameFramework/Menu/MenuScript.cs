using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {
    [SerializeField] Button startButton;
    public void NextMinigame() {
        MinigameManager.instance.GoToNextMinigameScene();
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
        PlayerJoinManager.onAllPlayersJoined.AddListener(OnAllPlayersConnected);
    }

    public void Update() {
        startButton.interactable = PlayerManager.AreAllPlayersConnected();
    }
    
    [SerializeField] List<PlayerSlotUI> playerSlots = new List<PlayerSlotUI>();
    
    public void OnPlayerConnected(int playerIndex) {
        playerSlots[playerIndex].SetStatus(true);
    }
    public void OnPlayerDisconnected(int playerIndex) {
        playerSlots[playerIndex].SetStatus(false);
    }

    private void OnAllPlayersConnected() {
        foreach(MultiplayerEventSystem eventSystem in FindObjectsOfType<MultiplayerEventSystem>()) {
            eventSystem.SetSelectedGameObject(startButton.gameObject);
        }
    }
}
