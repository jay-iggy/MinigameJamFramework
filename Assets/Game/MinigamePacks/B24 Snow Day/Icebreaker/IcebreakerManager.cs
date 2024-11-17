using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework;
using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;

public class IcebreakerManager : MonoBehaviour {
    

    public CameraController cameraController;
    public GameObject splashEffect;
    
    MinigameManager.Ranking ranking = new();
    GameObject lastDead;
    private bool ended = false;
    private int deaths = 0;

    private void Start() {
        ranking.SetRank(0, 1);
        ranking.SetRank(1, 1);
        ranking.SetRank(2, 1);
        ranking.SetRank(3, 1);
    }

    private void OnTriggerEnter(Collider other) {
        Pawn pawn = other.GetComponent<Pawn>();
        if(pawn != null) {
            lastDead = other.gameObject;
            KillPlayer(pawn);
        }
    }

    private void KillPlayer(Pawn pawn) {
        pawn.gameObject.GetComponent<SoundEffectController>().Play("Splash");
        pawn.gameObject.GetComponentInChildren<HummerController>().enabled = false;
        pawn.gameObject.GetComponentInChildren<HummerController>().highlight.SetActive(false);
        Instantiate(splashEffect, pawn.transform.position - new Vector3(0f,0.9f,0f), splashEffect.transform.rotation);
        pawn.gameObject.GetComponent<IcebreakerPawn>().alive = false;

        print($"Player Index: {pawn.playerIndex}");

        if (pawn.playerIndex >= 0)
        {
            ranking.SetRank(pawn.playerIndex, 4 - deaths);
        }

        deaths++;

        if (deaths >= 3) {
            StopMinigame();
        }
    }
    

    private void StopMinigame() {
        StartCoroutine(EndMinigame());
    }
    IEnumerator EndMinigame() {
        if (ended) {
            yield break;
        }
        ended = true;

        yield return new WaitForSeconds(2);
        
        StartCoroutine(cameraController.EndSequence(lastDead.gameObject));

        yield return new WaitForSeconds(5);
        MinigameManager.instance.EndMinigame(ranking);
    }
    
    
}
