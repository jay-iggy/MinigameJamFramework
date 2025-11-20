using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.Minigames;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MinigameUI : MonoBehaviour {
    public TextMeshProUGUI minigameNameText;
    public TextMeshProUGUI minigameDescriptionText;
    public TextMeshProUGUI minigameControlsText;
    public Image packIcon;
    public TextMeshProUGUI[] coloredHeaders = new TextMeshProUGUI[2];
    
    public void SetMinigame(MinigameInfo minigame) {
        minigameNameText.text = minigame.minigameName;
        minigameDescriptionText.text = minigame.description;
        minigameControlsText.text = minigame.controls;
        MinigamePack pack = minigame.GetPack();
        if (pack == null) return;
        packIcon.sprite = pack.icon;
        foreach (TextMeshProUGUI header in coloredHeaders) {
            header.color = pack.packColor;
        }
    }
}
