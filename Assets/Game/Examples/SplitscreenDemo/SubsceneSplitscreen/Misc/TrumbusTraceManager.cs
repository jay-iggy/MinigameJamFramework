using System;
using System.Collections;
using System.Collections.Generic;
using Examples.Splitscreen;
using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TrumbusTraceManager : MonoBehaviour {
    
    public static TrumbusTraceManager instance;
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    
    public List<TraceSubscene> subscenes = new();
    [SerializeField] GameObject _startText;
    [SerializeField] GameObject _endText;
    [SerializeField] TextMeshProUGUI _timerText;
    [SerializeField] Image _timerBackground;
    [SerializeField] Color _timerWarningColor;
    private bool _hasStarted = false;

    [SerializeField] private float _duration = 20;
    [SerializeField] private float _warningTime = 5f;
    
    void Start() {
        StartCoroutine(StartRoutine());
        PlayerManager.onPlayerConnected.AddListener(HandlePlayerJoined);
    }
    
    IEnumerator StartRoutine() {
        foreach (PlayerInput playerInput in PlayerManager.GetConnectedPlayerInputs()) {
            playerInput.currentActionMap.Disable();
        }
        yield return new WaitForSeconds(0.5f);
        _startText.SetActive(true);
        yield return new WaitForSeconds(1f);
        _startText.SetActive(false);
        _hasStarted = true;
        foreach (PlayerInput playerInput in PlayerManager.GetConnectedPlayerInputs()) {
            playerInput.currentActionMap.Enable();
        }
        StartCoroutine(TimerRoutine());
    }

    IEnumerator TimerRoutine() {
        float timeLeft = _duration;
        while (timeLeft > 0) {
            _timerText.text = Mathf.CeilToInt(timeLeft).ToString();
            yield return new WaitForSeconds(1f);
            timeLeft -= 1f;
            
            if (timeLeft <= _warningTime && _timerBackground.color != _timerWarningColor) {
                _timerBackground.color = _timerWarningColor;
            }
        }
        _timerText.text = "0";
        
        StartCoroutine(EndRoutine());
    }

    IEnumerator EndRoutine() {
        foreach (PlayerInput playerInput in PlayerManager.GetConnectedPlayerInputs()) {
            playerInput.currentActionMap.Disable();
        }
        _timerBackground.gameObject.SetActive(false);
        _endText.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        _endText.SetActive(false);

        foreach (TraceSubscene subscene in subscenes) {
            float score = subscene.CalculateAndDisplayScore();
            Debug.Log($"Player {subscene.playerIndex + 1} Score: {score}");
        }
    }
    
    private void HandlePlayerJoined(int playerIndex) {
        if (!_hasStarted) {
            PlayerInput playerInput = PlayerManager.players[playerIndex].playerInput;
            playerInput.currentActionMap.Disable();
        }
    }
    
}
