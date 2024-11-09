using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework;
using Game.MinigameFramework.Scripts.Framework.Input;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;

public class ExampleGameManager : MonoBehaviour {
    public float duration = 30;
    public float timer = 0;
    
    private List<Player> alivePlayers = new();
    
    MinigameManager.Ranking ranking = new();
    

    private void Start() {
        alivePlayers.AddRange(PlayerManager.players);

        StartCoroutine(GameTimer());

        //if not starting in editor (or if all players are bound ahead of time)
        // stop time + player input
        // 3 2 1 countdown
        // start time + player input
    }

    private void OnTriggerEnter(Collider other) {
        Pawn pawn = other.GetComponent<Pawn>();
        if(pawn != null) {
            KillPlayer(pawn);
        }
    }

    private void KillPlayer(Pawn pawn) {
        print($"Player Index: {pawn.playerIndex}");
        if(pawn.playerIndex < 0) return;
        
        Player player = PlayerManager.players[pawn.playerIndex];
        alivePlayers.Remove(player);
        ranking.AddFromEnd(player.playerIndex); // add player to lowest available rank
        
        if (alivePlayers.Count <= 1) {
            StopMinigame();
        }
    }

    IEnumerator GameTimer() {
        while (timer < duration) {
            timer += Time.deltaTime;
            yield return null;
        }
        StopMinigame();
    }

    

    private void StopMinigame() {
        StartCoroutine(EndMinigame());
    }
    IEnumerator EndMinigame() {
        // "FINISH" ui
        foreach(Player player in alivePlayers) {
            ranking.SetRank(player.playerIndex, 1);
        }
        yield return new WaitForSeconds(2);
        MinigameManager.instance.EndMinigame(ranking);
    }
    
    
}
