using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.MinigameFramework.Scripts;
using Game.MinigameFramework.Scripts.Framework.Minigames;
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
    

    public void GoToMainMenu() {
        SceneManager.LoadScene(mainMenuScene.SceneName);
    }

    private void Start() {
        // display all packs
        float next = 0f;
        foreach (MinigamePack pack in allPacks) {
            HoverIcon hi = Instantiate(hoverableIcon, packageIcons).GetComponent<HoverIcon>();
            hi.SetData(this, pack);
            hi.GetComponent<Image>().sprite = pack.icon;

            RectTransform rt = hi.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(next, 0);
            next += 112.5f;
        }
        // Start with no minigames displayed
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
            hi.GetComponent<Image>().sprite = game.thumbnail;

            RectTransform rt = hi.GetComponent<RectTransform>();
            rt.anchoredPosition = new Vector2(next, 0);
            next += 112.5f;
        }
    }
    
    public void OnPackHovered(MinigamePack pack) {
        Debug.Log(pack.description);
        description.text = pack.description;
        DisplayMinigames(pack);
    }
    
    public void OnMinigameHovered(MinigameInfo minigame) {
        description.text = minigame.description;
    }
}
