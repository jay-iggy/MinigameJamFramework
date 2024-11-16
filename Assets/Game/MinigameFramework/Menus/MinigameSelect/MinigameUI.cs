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
    
    public void SetMinigame(MinigameInfo minigame) {
        minigameNameText.text = minigame.minigameName;
        minigameDescriptionText.text = minigame.description;
        minigameControlsText.text = minigame.controls;
        packIcon.sprite = GetPackIcon(minigame);
    }
    private Sprite GetPackIcon(MinigameInfo minigame) {
        foreach (MinigamePack pack in MinigameManager.instance.minigamePacks) {
            if(pack.minigames.Contains(minigame)) {
                return pack.icon;
            }
        }
        return null;
    }
}
