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

    public GameObject hoverableIcon;
    public SceneField mainMenuScene;
    public List<MinigamePack> allPacks = new List<MinigamePack>();
    
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
    }

    private void DisplayPacks() {
        // display all packs
        float next = 0f;
        foreach (MinigamePack pack in allPacks) {
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
            img.color = MinigameManager.instance.PackIsOn(allPacks[i]) ? Color.white : _deselectedColor;
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
        description.text = pack.packName + "\n\t" + pack.description;
        DisplayMinigames(pack);
    }
    
    public void OnMinigameHovered(MinigameInfo minigame) {
        description.text = minigame.minigameName + "\n\t" + minigame.description + "\n\n" + minigame.credits;
    }
    
    public void TogglePack(MinigamePack pack) {
        MinigameManager.instance.TogglePack(pack);
        RefreshPackColors(); // refresh packs to have no grey or not grey
    }
}
