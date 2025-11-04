using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.Minigames;
using UnityEngine;
using UnityEngine.UI;

public class CreditsManager : MonoBehaviour {
    [SerializeField] private Transform parent;
    [SerializeField] private CreditUI packPrefab;
    [SerializeField] private CreditUI minigamePrefab;


    private void Start() {
        foreach (MinigamePack pack in MinigameManager.instance.allPacks) {
            CreatePackUI(pack);
        }

        Canvas.ForceUpdateCanvases();
    }

    private void CreatePackUI(MinigamePack pack) {
        CreditUI packCredit = Instantiate(packPrefab, parent);
        packCredit.transform.SetSiblingIndex(parent.childCount - 2);
        packCredit.SetText(pack.packName, pack.packColor, null);
        foreach (MinigameInfo minigame in pack.minigames) {
            CreateMinigameUI(minigame, pack.packColor, packCredit.childParent);
        }
    }

    private void CreateMinigameUI(MinigameInfo minigame, Color color, Transform parentTransform) {
        CreditUI minigameCredit = Instantiate(minigamePrefab, parentTransform);
        minigameCredit.SetText(minigame.minigameName, color, $"{minigame.credits}\n{minigame.attributions}");
    }
}
