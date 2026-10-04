using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

namespace Game.MinigameFramework.Menus.PromptWindow {
    public class PromptWindow : MonoBehaviour {
        [SerializeField] TextMeshProUGUI[] textObjs;
        [SerializeField] GameObject buttonParent;
        public UnityEvent onConfirm = new();
        public UnityEvent onCancel = new();

        private void Start() {
            PlayerManager.SetMenuActionMap();
            SetDefaultSelectedButton();   
        }
        
        public void SetText(string str) {
            foreach (TextMeshProUGUI text in textObjs) {
                text.text = str;
            }
        }

        private void SetDefaultSelectedButton() {
            GameObject button = buttonParent.transform.GetChild(0).gameObject;
            PlayerManager.SetSelectedGameObject(button);
        }

        public void ExitPrompt() {
            onCancel.Invoke();
            Destroy(gameObject);
        }
        public void Confirm() {
            onConfirm.Invoke();
        }
    }
}