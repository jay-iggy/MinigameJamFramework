using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Menus.MainMenu;
using Game.MinigameFramework.Scripts;
using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour {
    [SerializeField] Button startButton;
    [SerializeField] private Button packSelectButton;
    [SerializeField] RectTransform packageIcons;
    public GameObject packageIconPrefab;
    public SceneField packSelectScene;

    // Enforce Player Count replaced by context from MinigamePacks listed under MinigameManager
    // [SerializeField] bool enforcePlayerCount = true;
    
    public void NextMinigame() {
        MinigameManager.instance.PopulateMinigameList(); // populate list right before the rounds start
        MinigameManager.instance.GoToMinigameSelectScene();
    }
    
    public void ChangePacks() {
        SceneManager.LoadScene(packSelectScene.SceneName);
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
        
        foreach(Player player in PlayerManager.players) {
            if(player.playerInput!=null) {
                OnPlayerConnected(player.playerIndex);
            }
        }

        if (PlayerManager.AreAllPlayersConnected()) {
            OnAllPlayersConnected();
        }

        // display selected icons
        float next = 0f;
        float gap = 12.5f;
        foreach (Sprite spr in MinigameManager.instance.GetPackageSprites()) {
            RectTransform icon = Instantiate(packageIconPrefab, packageIcons).GetComponent<RectTransform>();

            Image img = icon.GetComponent<Image>();
            img.sprite = spr;

            icon.anchoredPosition = new Vector2(icon.anchoredPosition.x, next);
            next += icon.rect.height + gap;
        }
    }


    // unlocks button as needed, called when player connects or disconnects
    public void CheckUnlockButton() {
        startButton.interactable = PlayerManager.AreAllPlayersConnected();
        if (startButton.interactable) {
            PlayerManager.SetSelectedGameObject(startButton.gameObject);
        }
        else {
            PlayerManager.SetSelectedGameObject(packSelectButton.gameObject);
        }
    }
    
    [SerializeField] List<MenuPlayerSlot> playerSlots = new();
    
    public void OnPlayerConnected(int playerIndex) {
        playerSlots[playerIndex].SetStatus(true);
        playerSlots[playerIndex].BindToPlayer(playerIndex);
        CheckUnlockButton();
    }
    public void OnPlayerDisconnected(int playerIndex) {
        playerSlots[playerIndex].SetStatus(false);
        playerSlots[playerIndex].UnBindFromPlayer();
        CheckUnlockButton();
    }

    private void OnAllPlayersConnected() {
        PlayerManager.SetSelectedGameObject(startButton.gameObject);
    }

}
