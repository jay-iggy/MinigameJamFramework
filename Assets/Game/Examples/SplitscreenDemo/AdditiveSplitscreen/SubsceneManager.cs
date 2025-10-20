using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts;
using Game.MinigameFramework.Scripts.Framework;
using Game.MinigameFramework.Scripts.Framework.Input;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SubsceneManager : MonoBehaviour {
    [SerializeField] SceneField parentScene; 
    [SerializeField] Pawn pawn;
    [SerializeField] Camera cam;
    public int playerIndex = -1;
    
    private void OnEnable() {
        // Load parent scene if game is starting from subscene directly
        if (MinigameManager.instance == null) {
            SceneManager.LoadScene(parentScene.SceneName);
            return;
        }
        
        SetSubscenePosition();
        SetupSubsceneCamera();
        BindPawn();
    }


    private void SetSubscenePosition() {
        Vector3 subscenePosition = transform.position;
        subscenePosition.x += 100 * SplitscreenManager.instance.loadedPlayers;
        transform.position = subscenePosition;
    }

    private void BindPawn() {
        PawnBindingManager.BindPlayerInputToPawn(SplitscreenManager.instance.loadedPlayers, pawn);
        pawn.GetComponentInChildren<MeshRenderer>().material = SplitscreenManager.instance.materials[SplitscreenManager.instance.loadedPlayers];
        playerIndex = SplitscreenManager.instance.loadedPlayers;
        SplitscreenManager.instance.loadedPlayers++;
    }

    private void SetupSubsceneCamera() {
        Vector4 camPosition = SplitscreenManager.instance.cameraPositions[SplitscreenManager.instance.loadedPlayers];
        cam.rect = new Rect(camPosition.x, camPosition.y, camPosition.z, camPosition.w);
    }
    
}
