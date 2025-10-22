using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class ResultsSceneDelay : MonoBehaviour {
    [SerializeField] private float delay = 5f;
    [SerializeField] private SceneField minigameSelectScene;
    [SerializeField] private SceneField victoryScene;

    IEnumerator Start() {
        yield return new WaitForSeconds(delay);
        if (HasPlayerWon()) {
            MinigameManager.instance.ResetPoints();
            MinigameManager.instance.PopulateMinigameList();
            SceneManager.LoadScene(victoryScene.SceneName);
        }
        else {
            SceneManager.LoadScene(minigameSelectScene.SceneName);
        }
    }

    private bool HasPlayerWon() {
        foreach (Player player in PlayerManager.players) {
            if (player.points >= MinigameManager.instance.pointsToWin) {
                return true;
            }
        }
        return false;
    }
}
