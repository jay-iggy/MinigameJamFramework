using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour {
    public void LoadRandomMinigame() {
        MinigameManager.instance.LoadRandomMinigame();
    }
    
    List<PlayerSlot> playerSlots = new List<PlayerSlot>();
    
    public void SetPlayerSlotStatus(int playerIndex, bool connected) {
        playerSlots[playerIndex].SetStatus(connected);
    }
}
