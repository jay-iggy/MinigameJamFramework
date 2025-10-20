using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplitscreenManager : MonoBehaviour {
    [SerializeField] private SceneField scene;
    
    private void Start() {
        LoadAdditiveScene(scene.SceneName);
    }
    
    
    public void LoadAdditiveScene(string sceneName) {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
    
}
