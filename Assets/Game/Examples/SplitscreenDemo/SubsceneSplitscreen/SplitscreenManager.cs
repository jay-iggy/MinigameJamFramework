using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplitscreenManager : MonoBehaviour {
    [SerializeField] private GameObject subscenePrefab;
    
    public static SplitscreenManager instance;
    
    [HideInInspector] public int loadedPlayers = 0;
    
    public List<Material> materials = new();
    
    public CameraRect[] cameraRect = {new CameraRect(0,0.5f), new CameraRect(0.5f,0.5f), new CameraRect(0,0), new CameraRect(0.5f,0)};
    
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


    [Serializable]
    public class CameraRect {
        public float x,y,width=0.5f,height=0.5f;
        public CameraRect(float x, float y, float width, float height) {
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
        }

        public CameraRect(float x, float y) {
            this.x = x;
            this.y = y;
        }
    }
}
