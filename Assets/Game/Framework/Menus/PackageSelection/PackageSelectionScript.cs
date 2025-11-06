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
    

    public void GoToMainMenu() {
        MinigameManager.instance.PopulateMinigameList();
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
        RefreshStatusText(MinigameManager.instance.IsPackOn(MinigameManager.instance.allPacks[0]));
    }

    private void DisplayPacks() {
        // display all packs
        float next = 0f;
        foreach (MinigamePack pack in MinigameManager.instance.allPacks) {
            HoverIcon hi = Instantiate(hoverableIcon, packageIcons).GetComponent<HoverIcon>();
            hi.SetData(this, pack);

            Image img = hi.GetComponent<Image>();
            img.sprite = pack.icon;

            RectTransform rt = hi.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(next, 0);
            next += 112.5f;
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
        RefreshStatusText(MinigameManager.instance.IsPackOn(pack));
    }
    
    public void OnMinigameHovered(MinigameInfo minigame) {
        description.text = $"<color=#{ColorUtility.ToHtmlStringRGB(minigame.GetPack().packColor)}><size=150%><b>{minigame.minigameName.ToUpper()}</b></size></color>\n\n{minigame.description}\n\n{minigame.credits}";
        enableStatusText.text = "";
        RefreshStatusText(MinigameManager.instance.IsMinigameOn(minigame));
    }
    
    public void TogglePack(MinigamePack pack) {
        MinigameManager.instance.TogglePack(pack);
        RefreshStatusText(MinigameManager.instance.IsPackOn(pack));
    }

    private void RefreshStatusText(bool condition) {
        enableStatusText.text = condition ? "Enabled" : "Disabled";
        enableStatusText.color = condition ? enabledColor : disabledColor;
    }

    public void ToggleMinigame(MinigameInfo minigameData) {
        MinigameManager.instance.ToggleMinigame(minigameData);
        RefreshStatusText(MinigameManager.instance.IsMinigameOn(minigameData));
    }
}
