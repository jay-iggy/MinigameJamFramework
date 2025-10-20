using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class PauseMenuButtons : MonoBehaviour {
    [SerializeField] GameObject buttonParent;

    private void Start() {
        PlayerManager.SetMenuActionMap();
        SetDefaultSelectedButton();
    }
    
    private void SetDefaultSelectedButton() {
        GameObject button = buttonParent.transform.GetChild(0).gameObject;
        PlayerManager.SetSelectedGameObject(button);
    }

    public void ResumeGame() {
        Time.timeScale = 1;
        PlayerManager.SetMinigameActionMap();
        Destroy(gameObject);
    }
    public void ReturnToMainMenu() {
        Time.timeScale = 1;
        Destroy(gameObject);
        MinigameManager.instance.GoToMainMenuScene();
    }
}
