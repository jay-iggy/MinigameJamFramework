using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Game.MinigameFramework.Menus.MainMenu {
    public class MenuPlayerSlot : PlayerSlotUI {
        [Header("Disconnect")]
        [SerializeField] float heldDuration = 1f;
        [SerializeField] private Image radialImage;

        [Header("Button Prompt")] [SerializeField]
        private Image promptButtonRenderer;
        [SerializeField] Sprite keyboardSprite;
        [SerializeField] Sprite buttonSprite;
        
        private int _playerIndex = -1;
        private Coroutine _timerCoroutine;

        private void Awake() {
            radialImage.gameObject.SetActive(false);
        }

        public void BindToPlayer(int index) {
            _playerIndex = index;
            PlayerInput playerInput = PlayerManager.players[_playerIndex].playerInput;
            playerInput.currentActionMap.actionTriggered += HandleDisconnectInput;
            promptButtonRenderer.sprite = playerInput.currentControlScheme == "Gamepad" ? buttonSprite:keyboardSprite;
        }

        public void UnBindFromPlayer() {
            _playerIndex = -1;
        }

        private void HandleDisconnectInput(InputAction.CallbackContext context) {
            if (context.action.name != "Cancel") return;

            // On Press
            if (context.action.WasPerformedThisFrame()) {
                _timerCoroutine = StartCoroutine(TimerCoroutine());
            }
            // On Release
            else {
                if (_timerCoroutine != null) {
                    StopCoroutine(_timerCoroutine);
                    radialImage.gameObject.SetActive(false);
                }
            }
        }

        private IEnumerator TimerCoroutine() {
            float  timer = 0f;
            radialImage.gameObject.SetActive(true);
            while (timer < heldDuration) {
                timer += Time.deltaTime;
                radialImage.fillAmount = timer / heldDuration;
                yield return null;
            }
            _timerCoroutine = null;
            
            radialImage.gameObject.SetActive(false);
            DisconnectPlayer();
        }

        private void DisconnectPlayer() {
            Destroy(PlayerManager.players[_playerIndex].playerInput.gameObject);
        }
    }
}