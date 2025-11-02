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
    private void Start() {
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
        StartCoroutine(MoveToPosition(isConnected ? new Vector2(_defaultPosition.x, raisedYPosition) : _defaultPosition));
    }
}
