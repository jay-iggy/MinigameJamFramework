using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts;
using Game.MinigameFramework.Scripts.Framework.Minigames;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }
    
    public SceneField nextMinigameScene;
    public SceneField resultsScene;
    
    [Header("Points")]
    public int pointsToWin = 20;
    public int pointsForFirstPlace = 4;
    public int pointsForSecondPlace = 3;
    public int pointsForThirdPlace = 2;
    public int pointsForFourthPlace = 1;
    
    [Header("Minigames")]
    public List<MinigameInfo> minigames;
    
    /*public void LoadRandomMinigame() {
        int randomIndex = UnityEngine.Random.Range(0, minigames.Count);
        LoadMinigame(minigames[randomIndex]);
    }*/
    
    public void GoToNextMinigameScene() {
        SceneManager.LoadScene(nextMinigameScene.SceneName);
    }
    
    public void LoadMinigame(MinigameInfo minigame) {
        SceneManager.LoadScene(minigame.scene.SceneName);
        PlayerManager.SetMinigameActionMap();
    }
    
    
    public void EndMinigame(Ranking ranking) {
        AwardPoints(ranking.playerRanks);
        PlayerManager.SetMenuActionMap();
        SceneManager.LoadScene(resultsScene.SceneName);
    }

    private void AwardPoints(List<int> ranking) {
        for(int playerIndex = 0; playerIndex < PlayerManager.players.Count; playerIndex++) {
            int playerRank = ranking[playerIndex];
            PlayerManager.players[playerIndex].points += GetPointsForRank(playerRank);
        }
    }
    private int GetPointsForRank(int rank) {
        switch(rank) {
            case 0:
                return pointsForFirstPlace;
            case 1:
                return pointsForSecondPlace;
            case 2:
                return pointsForThirdPlace;
            case 3:
                return pointsForFourthPlace;
            default:
                return 0;
        }
    }
    
    public class Ranking {
        public List<int> playerRanks = new(4); // index in list is playerIndex, value is rank

        public Ranking() {
            playerRanks.Add(0);
            playerRanks.Add(0);
            playerRanks.Add(0);
            playerRanks.Add(0);
        }
        
        public void AddFromEnd(int playerIndex) {
            playerRanks[playerIndex] = GetNextLowestRank();
        }
        public void AddFromStart(int playerIndex) {
            playerRanks[playerIndex] = GetNextHighestRank();
        }
        
        public int GetNextHighestRank() {
            int nextHighestRank = 1;
            
            if(playerRanks.Contains(1)) {
                nextHighestRank = 2;
            } else if(playerRanks.Contains(2)) {
                nextHighestRank = 3;
            } else if(playerRanks.Contains(3)) {
                nextHighestRank = 4;
            }
            
            return nextHighestRank;
        }
        public int GetNextLowestRank() {
            // if theres a 4, return 3
            // if theres a 3, return 2
            // if theres a 2, return 1
            int nextLowestRank = 4;
            
            if (playerRanks.Contains(4)) {
                nextLowestRank = 3;
            } else if (playerRanks.Contains(3)) {
                nextLowestRank = 2;
            } else if (playerRanks.Contains(2)) {
                nextLowestRank = 1;
            }
            return nextLowestRank;
        }
        public void SetRank(int playerIndex, int rank) {
            playerRanks[playerIndex] = rank;
        }
    }
    
}
