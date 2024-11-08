using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsSceneDelay : MonoBehaviour {
    [SerializeField] private float delay = 5f;
    [SerializeField] private SceneField nextMinigameScene;

    IEnumerator Start() {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(nextMinigameScene.SceneName);
    }
}
