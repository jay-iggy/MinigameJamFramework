using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSlotUI : MonoBehaviour {
    public TextMeshProUGUI statusText;
    [SerializeField] string statusConnected = "Connected";
    [SerializeField] string statusNotConnected = "Press Any Button";
    [SerializeField] Color connectedColor = Color.green;
    [SerializeField] Color notConnectedColor = Color.white;

    [SerializeField] private float raisedYPosition = 0f;
    [SerializeField] private float animationSpeed = 10f;
    
    [SerializeField] RectTransform rectTransform;
    private Vector2 _defaultPosition;

    private Coroutine _animationCoroutine = null;

    [Header("SFX")]
    [SerializeField] AudioClip confirmSfx;
    [SerializeField] AudioClip cancelSfx;
    protected AudioSource _audioSource;


    protected virtual void Awake() {
        _audioSource = GetComponent<AudioSource>();
    }

    protected virtual void Start() {
        _defaultPosition = rectTransform.anchoredPosition;
    }
    
    IEnumerator MoveToPosition(Vector2 targetPosition) {
        while (Vector2.Distance(rectTransform.anchoredPosition, targetPosition) > 0.01f) {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, Time.deltaTime * animationSpeed);
            yield return null;
        }
        rectTransform.anchoredPosition = targetPosition;
    }
    
    public void SetStatus(bool isConnected) {
        statusText.text = isConnected ? statusConnected : statusNotConnected;
        statusText.color = isConnected ? connectedColor : notConnectedColor;
        _audioSource?.PlayOneShot(isConnected?confirmSfx:cancelSfx);
        if(_animationCoroutine!=null) StopCoroutine(_animationCoroutine);
        _animationCoroutine = StartCoroutine(MoveToPosition(isConnected ? new Vector2(_defaultPosition.x, raisedYPosition) : _defaultPosition));
    }
}
