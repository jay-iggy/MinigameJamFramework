using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.MinigameFramework.Scripts;
using Game.MinigameFramework.Scripts.Framework.Minigames;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PackageSelectionScript : MonoBehaviour
{
    [SerializeField] Transform packageIcons;
    [SerializeField] Transform minigameIcons;
    [SerializeField] TMP_Text description;
    [SerializeField] private TextMeshProUGUI enableStatusText;
    [SerializeField] private Color enabledColor;
    [SerializeField] private Color disabledColor;

    public GameObject hoverableIcon;
    public SceneField mainMenuScene;
    
    private Color _deselectedColor = new Color(.5f, .5f, .5f, 1.0f);
    

    public void GoToMainMenu() {
        SceneManager.LoadScene(mainMenuScene.SceneName);
    }

    private void Start() {
        DisplayPacks();
        
        if(PlayerManager.GetNumPlayers() > 0) {
            SetInitialSelection();
        } else {
            PlayerManager.onPlayerConnected.AddListener(SetInitialSelection);
        }
    }

    private void SetInitialSelection(int index = 0) {
        PlayerManager.SetSelectedGameObject(packageIcons.GetChild(0).gameObject);
        PlayerManager.onPlayerConnected.RemoveListener(SetInitialSelection);
        RefreshStatusText(MinigameManager.instance.minigamePacks[0]);
    }

    private void DisplayPacks() {
        // display all packs
        float next = 0f;
        foreach (MinigamePack pack in MinigameManager.instance.allPacks) {
            HoverIcon hi = Instantiate(hoverableIcon, packageIcons).GetComponent<HoverIcon>();
            hi.SetData(this, pack);

            Image img = hi.GetComponent<Image>();
            img.sprite = pack.icon;
            if (!MinigameManager.instance.PackIsOn(pack)) img.color = _deselectedColor;

            RectTransform rt = hi.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(next, 0);
            next += 112.5f;
        }
    }

    private void RefreshPackColors() {
        for (int i = 0; i < packageIcons.childCount; i++) {
            Image img = packageIcons.GetChild(i).GetComponent<Image>();
            img.color = MinigameManager.instance.PackIsOn(MinigameManager.instance.allPacks[i]) ? Color.white : _deselectedColor;
        }
    }
    
    private void DisplayMinigames(MinigamePack pack) {
        // clear minigames
        foreach (Transform child in minigameIcons.transform) {
            Destroy(child.gameObject);
        }
        // replace with pack contents
        float next = 0f;
        foreach (MinigameInfo game in pack.minigames) {
            HoverIcon hi = Instantiate(hoverableIcon, minigameIcons).GetComponent<HoverIcon>();
            hi.SetData(this, game);

            Image img = hi.GetComponent<Image>();
            img.sprite = game.thumbnail;

            RectTransform rt = hi.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(next, 0);
            next += 112.5f;
        }
    }
    
    public void OnPackHovered(MinigamePack pack) {
        description.text = $"<color=#{ColorUtility.ToHtmlStringRGB(pack.packColor)}><size=150%><b>{pack.packName.ToUpper()}</b></size></color>\n\n{pack.description}";
        DisplayMinigames(pack);
        RefreshStatusText(pack);
    }
    
    public void OnMinigameHovered(MinigameInfo minigame) {
        description.text = $"<color=#{ColorUtility.ToHtmlStringRGB(minigame.GetPack().packColor)}><size=150%><b>{minigame.minigameName.ToUpper()}</b></size></color>\n\n{minigame.description}\n\n{minigame.credits}";
        enableStatusText.text = "";
    }
    
    public void TogglePack(MinigamePack pack) {
        MinigameManager.instance.TogglePack(pack);
        RefreshPackColors(); // refresh packs to have no grey or not grey
        RefreshStatusText(pack);
    }

    private void RefreshStatusText(MinigamePack pack) {
        bool status = MinigameManager.instance.PackIsOn(pack);
        enableStatusText.text = status ? "Enabled" : "Disabled";
        enableStatusText.color = status ? enabledColor : disabledColor;
    }
}
