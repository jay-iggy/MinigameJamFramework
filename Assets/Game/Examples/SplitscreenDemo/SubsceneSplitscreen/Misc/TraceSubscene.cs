using System;
using System.Collections;
using Examples.Splitscreen;
using TMPro;
using UnityEngine;

public class TraceSubscene : MonoBehaviour {
    [SerializeField] SubsceneManager subsceneManager;
    public int playerIndex { get; private set; }
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] private Stencil stencil;

    private void Awake() {
        TrumbusTraceManager.instance.subscenes.Add(this);
        playerIndex = subsceneManager.playerIndex;
    }
    
    public float CalculateAndDisplayScore() {
        float score = stencil.CalculateScore();
        float percentage = score * 100f;
        scoreText.text = $"{Mathf.RoundToInt(percentage)}%";
        return score;
    }
}
