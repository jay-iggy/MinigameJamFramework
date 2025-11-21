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
    }

    private void CreatePackUI(MinigamePack pack) {
        CreditUI packCredit = Instantiate(packPrefab, parent);
        packCredit.transform.SetSiblingIndex(parent.childCount - 2);
        packCredit.SetText(pack.packName, pack.packColor, null);
        foreach (MinigameInfo minigame in pack.minigames) {
            CreateMinigameUI(minigame, pack.packColor, packCredit.childParent);
        }
        
        // Fix issue where children don't adjust to parent's position
        // Band-aid fix, you can see a jarring visual bug frame where its wrong
        LayoutRebuilder.ForceRebuildLayoutImmediate(packCredit.childParent.GetComponent<RectTransform>());
    }

    private void CreateMinigameUI(MinigameInfo minigame, Color color, Transform parentTransform) {
        CreditUI minigameCredit = Instantiate(minigamePrefab, parentTransform);
        minigameCredit.SetText(minigame.minigameName, color, $"{minigame.credits}\n{minigame.attributions}");
    }
}
