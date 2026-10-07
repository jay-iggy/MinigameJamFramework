using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using Game.MinigameFramework.Scripts.Framework.Minigames;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using Unity.VisualScripting;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HoverIcon : Selectable, IPointerClickHandler, ISubmitHandler, ICancelHandler
{
    // keeps a reference to PackageSelectionScript
    private PackageSelectionScript pss;
    
    private MinigamePack packData;
    private MinigameInfo minigameData;
    [SerializeField] Color packHighlightColor = new Color(0,0,0,0.25f);
    [SerializeField] Color minigameHighlightColor = new Color(1f,1f,0,0.5f);
    private float pressCooldown = 0.1f; // cooldown prevents from being toggled twice from the same input
    private float nextPressTime = 0;

    // UI Events
    public override void OnSelect(BaseEventData eventData) {
        base.OnSelect(eventData);
        OnIconHovered();
    }
    public override void OnPointerEnter(PointerEventData eventData) {
        base.OnPointerEnter(eventData);
        OnIconHovered();
    }
    public void OnPointerClick(PointerEventData eventData) => OnIconPressed();
    public void OnSubmit(BaseEventData eventData) => OnIconPressed();
    
    // Internal methods
    private void OnIconHovered() {
        if (packData != null) pss.OnPackHovered(packData);
        if (minigameData != null) pss.OnMinigameHovered(minigameData);
    }
    private void OnIconPressed() {
        if(Time.time < nextPressTime) return;
        if (packData != null) pss.TogglePack(packData);
        nextPressTime = Time.time + pressCooldown;
    }
    
    public void SetData(PackageSelectionScript pssSet, MinigamePack pd) {
        packData = pd;
        minigameData = null;
        pss = pssSet;
        targetGraphic.color = pd.packColor;
    }
    public void SetData(PackageSelectionScript pssSet, MinigameInfo md) {
        minigameData = md;
        packData = null;
        pss = pssSet;
        targetGraphic.color = md.GetPack().packColor;
    }

    public void OnCancel(BaseEventData eventData) {
        PlayerManager.SetSelectedGameObject(FindSelectableOnUp().gameObject);
    }
}
