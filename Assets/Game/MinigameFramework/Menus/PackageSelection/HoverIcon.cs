using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Game.MinigameFramework.Scripts.Framework.Minigames;

public class HoverIcon : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    // keeps a reference to PackageSelectionScript
    private PackageSelectionScript pss;

    private MinigamePack packData;
    private MinigameInfo minigameData;

    public void OnPointerEnter(PointerEventData eventData) {
        if (packData != null) pss.OnPackHovered(packData);
        if (minigameData != null) pss.OnMinigameHovered(minigameData);
    }

    public void SetData(PackageSelectionScript pssSet, MinigamePack pd) {
        packData = pd;
        minigameData = null;
        pss = pssSet;
    }
    
    public void SetData(PackageSelectionScript pssSet, MinigameInfo md) {
        minigameData = md;
        packData = null;
        pss = pssSet;
    }
    
    public void OnPointerDown(PointerEventData eventData) {
        if (packData != null) pss.PackToggled(packData);
        // could be extended to toggling specific minigames, but would require some MinigameManager reworks
    }
}
