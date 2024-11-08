using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.Minigames;
using TMPro;
using UnityEngine;

public class MinigameUI : MonoBehaviour {
    public TextMeshProUGUI minigameNameText;
    public TextMeshProUGUI minigameDescriptionText;
    public TextMeshProUGUI minigameControlsText;
    
    public void SetMinigame(MinigameInfo minigame) {
        minigameNameText.text = minigame.minigameName;
        minigameDescriptionText.text = minigame.description;
        minigameControlsText.text = minigame.controls;
    }
}
