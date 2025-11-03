using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CancelNavigation : MonoBehaviour, ICancelHandler {
    private Selectable selectable;
    [SerializeField] Vector3 direction = Vector3.up;

    private void Awake() {
        selectable = GetComponent<Selectable>();
    }

    public void OnCancel(BaseEventData eventData) {
        PlayerManager.SetSelectedGameObject(selectable.FindSelectable(direction).gameObject);
    }

}
