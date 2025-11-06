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
using Image = UnityEngine.UI.Image;

public class HoverIcon : Selectable, IPointerClickHandler, ISubmitHandler, ICancelHandler {
    // keeps a reference to PackageSelectionScript
    private PackageSelectionScript _pss;
    
    private MinigamePack _packData;
    private MinigameInfo _minigameData;
    [SerializeField] Color packHighlightColor = new Color(0,0,0,0.25f);
    [SerializeField] Color minigameHighlightColor = new Color(1f,1f,0,0.5f);
    private Image _image;
    private Color _deselectedColor = new Color(.5f, .5f, .5f, 1.0f);

    protected override void Awake() {
        base.Awake();
        _image = GetComponent<Image>();
    }


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
        if (_packData != null) _pss.OnPackHovered(_packData);
        if (_minigameData != null) _pss.OnMinigameHovered(_minigameData);
    }
    private void OnIconPressed() {
        if (_packData != null) {
            _pss.TogglePack(_packData);
            RefreshColor(MinigameManager.instance.IsPackOn(_packData));
        }
        if(_minigameData!=null) {
            _pss.ToggleMinigame(_minigameData);
            RefreshColor(MinigameManager.instance.IsMinigameOn(_minigameData));
        }
    }
    
    public void SetData(PackageSelectionScript pssSet, MinigamePack pd) {
        _packData = pd;
        _minigameData = null;
        _pss = pssSet;
        targetGraphic.color = packHighlightColor;
        RefreshColor(MinigameManager.instance.IsPackOn(pd));
    }
    public void SetData(PackageSelectionScript pssSet, MinigameInfo md) {
        _minigameData = md;
        _packData = null;
        _pss = pssSet;
        targetGraphic.color = minigameHighlightColor;
        RefreshColor(MinigameManager.instance.IsMinigameOn(_minigameData));
    }

    public void OnCancel(BaseEventData eventData) {
        PlayerManager.SetSelectedGameObject(FindSelectableOnUp().gameObject);
    }

    private void RefreshColor(bool condition) {
        _image.color = condition ? Color.white : _deselectedColor;
    }
}
