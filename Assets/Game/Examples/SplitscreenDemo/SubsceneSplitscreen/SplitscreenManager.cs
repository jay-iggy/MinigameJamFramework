using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplitscreenManager : MonoBehaviour {
    [SerializeField] private GameObject subscenePrefab;
    
    public static SplitscreenManager instance;
    
    public int loadedPlayers = 0;
    
    public List<Material> materials = new();

    public Vector4[] cameraPositions =
    {new Vector4(0, 0.5f,0.5f,0.5f), new Vector4(0.5f, 0.5f,0.5f,0.5f), new Vector4(0, 0,0.5f,0.5f), new Vector4(0.5f, 0,0.5f,0.5f)};
    
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }
    
    private void Start() {
        PlayerManager.SetMinigameActionMap();
        for(int i = 0; i< PlayerManager.players.Count; i++) {
            Instantiate(subscenePrefab);
        }
        
        PlayerManager.onPlayerConnected.AddListener(OnPlayerConnected);
    }

    private void OnPlayerConnected(int playerIndex) {
        if (playerIndex <= loadedPlayers) {
            Instantiate(subscenePrefab);
        }
    }


    public void LoadAdditiveScene(string sceneName) {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
    }
    
}
