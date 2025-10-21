using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts;
using Game.MinigameFramework.Scripts.Framework;
using Game.MinigameFramework.Scripts.Framework.Input;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SubsceneManager : MonoBehaviour {
    [SerializeField] Pawn pawn;
    [SerializeField] Camera cam;
    public int playerIndex = -1;
    
    private void OnEnable() {
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
        SplitscreenManager.CameraRect camRect = SplitscreenManager.instance.cameraRect[SplitscreenManager.instance.loadedPlayers];
        cam.rect = new Rect(camRect.x, camRect.y, camRect.width, camRect.height);
    }
    
}
