using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.Minigames;
using UnityEngine;

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
        packCredit.SetText(pack.packName, pack.packColor, pack.createdFor);
        foreach (MinigameInfo minigame in pack.minigames) {
            CreateMinigameUI(minigame, pack.packColor, packCredit.childParent);
        }
    }

    private void CreateMinigameUI(MinigameInfo minigame, Color color, Transform parentTransform) {
        CreditUI minigameCredit = Instantiate(minigamePrefab, parentTransform);
        minigameCredit.SetText(minigame.minigameName, color, $"{minigame.credits}\n{minigame.attributions}");
    }
}
