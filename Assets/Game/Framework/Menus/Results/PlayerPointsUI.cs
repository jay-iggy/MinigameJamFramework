using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;

public class PlayerPointsUI : MonoBehaviour {
    [SerializeField] private int index;
    public TMPro.TextMeshProUGUI pointsText;
    [SerializeField] private RectTransform bar;
    [SerializeField] private float animationSpeed = 10f;
    private float _minY;
    [SerializeField] private float _maxY;

    private void Start() {
        _minY = bar.anchoredPosition.y;
        if (index >= PlayerManager.players.Count) return; // debugging without proper player count
        
        SetPlayerPoints(PlayerManager.players[index].points);
    }


    public void SetPlayerPoints(int points) {
        pointsText.text = $"{points} PTS";
        StartCoroutine(MoveToPosition(GetBarPosition(points)));
    }

    private Vector2 GetBarPosition(int points) {
        float y;
        if (points == 0) {
            y = _minY;
        } else {
            y = Mathf.Lerp(_minY, _maxY, (float) points / MinigameManager.instance.pointsToWin);
        }

        return new Vector2(bar.anchoredPosition.x, y);
    }
    
    IEnumerator MoveToPosition(Vector2 targetPosition) {
        while (Vector2.Distance(bar.anchoredPosition, targetPosition) > 0.01f) {
            bar.anchoredPosition = Vector2.Lerp(bar.anchoredPosition, targetPosition, Time.deltaTime * animationSpeed);
            yield return null;
        }
        bar.anchoredPosition = targetPosition;
    }
}
