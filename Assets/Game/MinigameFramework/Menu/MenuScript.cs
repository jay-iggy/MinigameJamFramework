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

    public void Update() {
        startButton.interactable = PlayerManager.AreAllPlayersConnected();
    }
    
    List<PlayerSlot> playerSlots = new List<PlayerSlot>();
    
    public void SetPlayerSlotStatus(int playerIndex, bool connected) {
        playerSlots[playerIndex].SetStatus(connected);
    }
}
