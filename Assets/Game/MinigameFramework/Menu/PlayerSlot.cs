using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerSlot : MonoBehaviour {
    public TextMeshProUGUI statusText;
    [SerializeField] string statusConnected = "Connected";
    [SerializeField] string statusNotConnected = "Press Any Button";
    [SerializeField] Color connectedColor = Color.green;
    [SerializeField] Color notConnectedColor = Color.white;

    [SerializeField] private float raisedYPosition = 0f;
    [SerializeField] private float animationSpeed = 10f;
    
    [SerializeField] RectTransform rectTransform;
    Vector2 _defaultPosition;
    private void Start() {
        _defaultPosition = rectTransform.anchoredPosition;
    }
    
    IEnumerator MoveToPosition(Vector2 targetPosition) {
        Vector2 startPosition = rectTransform.anchoredPosition;
        while (Vector2.Distance(startPosition, targetPosition) > 0.1f) {
            rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, targetPosition, Time.deltaTime * animationSpeed);
            yield return null;
        }
        rectTransform.anchoredPosition = targetPosition;
    }
    
    public void SetStatus(bool connected) {
        statusText.text = connected ? statusConnected : statusNotConnected;
        statusText.color = connected ? connectedColor : notConnectedColor;
        StartCoroutine(MoveToPosition(connected ? _defaultPosition : new Vector2(_defaultPosition.x, raisedYPosition)));
    }
}
