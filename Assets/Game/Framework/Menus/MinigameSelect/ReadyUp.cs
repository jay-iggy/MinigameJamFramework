using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class ReadyUp : MonoBehaviour {
    [FormerlySerializedAs("onReady")] public UnityEvent onAllPlayersReady;
    private List<int> _readyPlayers = new List<int>();
    [SerializeField] private List<PlayerSlotUI> playerSlots;


    public void EnableReadyUp() {
        foreach(Player player in PlayerManager.players) {
            ReadyInput readyInput = new (player.playerIndex);
            readyInput.onReady.AddListener(SetPlayerAsReady);
        }
    }
    
    private void SetPlayerAsReady(int playerIndex) {
        if(_readyPlayers.Contains(playerIndex)) return;
        
        _readyPlayers.Add(playerIndex);
        playerSlots[playerIndex].SetStatus(true);
        
        if(_readyPlayers.Count == PlayerManager.players.Count) {
            onAllPlayersReady.Invoke();
        }
    }
}

public class ReadyInput : Object {
    public int playerIndex;
    public UnityEvent<int> onReady;
    
    public ReadyInput(int playerIndex) {
        this.playerIndex = playerIndex;
        PlayerManager.players[playerIndex].playerInput.onActionTriggered += OnReady;
        onReady = new UnityEvent<int>();
    }
    
    public void OnReady(InputAction.CallbackContext context) {
        if(context.action.type != InputActionType.Button) return;
        
        onReady.Invoke(playerIndex);
        PlayerManager.players[playerIndex].playerInput.onActionTriggered -= OnReady;
        // delete this object
        Destroy(this);
    }
}
