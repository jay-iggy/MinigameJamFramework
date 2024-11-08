using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.Minigames;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class NextMinigameScript : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI minigameNameText;
    [SerializeField]private int iterations = 20;
    [SerializeField] private AnimationCurve speedCurve;
    [SerializeField] private float baseSpeed = 0.2f;
    [SerializeField] private Color selectedColor = Color.green;
    [SerializeField] float descriptionDelay = 2f;
    [SerializeField]float loadSceneDelay = 1f;
    [SerializeField] MinigameUI minigameUI;
    
    public UnityEvent onMinigameSelected;
    
    MinigameInfo minigame;

    private IEnumerator Start() {
        List<MinigameInfo> minigames = MinigameManager.instance.minigames;
        int minigameIndex = 0;
        
        if(minigames.Count == 0) {
            minigameNameText.text = "None";
            minigameNameText.color = Color.red;
            yield break;
        }
        
        if(minigames.Count == 1) {
            minigameNameText.text = minigames[0].minigameName;
            minigameNameText.color = selectedColor;
        }
        else {
            for(int i = 0; i<iterations; i++) {
                minigameIndex = UnityEngine.Random.Range(0, minigames.Count);
                minigameNameText.text = minigames[minigameIndex].name;
                yield return new WaitForSeconds(baseSpeed * speedCurve.Evaluate(i/iterations));
            }
        }
        
        minigame = minigames[minigameIndex];
        minigameNameText.color = selectedColor;
        StartCoroutine(AfterSlotsEnd());
    }

    IEnumerator AfterSlotsEnd() {
        yield return new WaitForSeconds(descriptionDelay);
        onMinigameSelected.Invoke();
        minigameUI.SetMinigame(minigame);
    }
    
    
    public void OnPlayersReady() {
        StartCoroutine(LoadMinigame());
    }

    IEnumerator LoadMinigame() {
        yield return new WaitForSeconds(loadSceneDelay);
        MinigameManager.instance.LoadMinigame(minigame);
    }
}
