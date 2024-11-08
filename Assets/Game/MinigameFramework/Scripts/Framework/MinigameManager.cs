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
    
    public void EndMinigame(ResultsRanking ranking) {
        AwardPoints(ranking);
        PlayerManager.SetMenuActionMap();
        SceneManager.LoadScene(resultsScene.SceneName);
    }

    private void AwardPoints(ResultsRanking ranking) {
        foreach (Player player in ranking.firstPlace) {
            player.points += pointsForFirstPlace;
        }
        foreach (Player player in ranking.secondPlace) {
            player.points += pointsForSecondPlace;
        }
        foreach (Player player in ranking.thirdPlace) {
            player.points += pointsForThirdPlace;
        }
        foreach (Player player in ranking.fourthPlace) {
            player.points += pointsForFourthPlace;
        }
    }
    private void AwardPoints(int playerIndex, int points) {
        PlayerManager.players[playerIndex].points += points;
    }
    public class ResultsRanking {
        public List<Player> firstPlace;
        public List<Player> secondPlace;
        public List<Player> thirdPlace;
        public List<Player> fourthPlace;
        
        public ResultsRanking() {
            firstPlace = new List<Player>();
            secondPlace = new List<Player>();
            thirdPlace = new List<Player>();
            fourthPlace = new List<Player>();
        }

        public void AddFromEnd(Player player) {
            if(fourthPlace.Count < 1) {
                fourthPlace.Add(player);
            } else if(thirdPlace.Count < 1) {
                thirdPlace.Add(player);
            } else if(secondPlace.Count < 1) {
                secondPlace.Add(player);
            } else if(firstPlace.Count < 1) {
                firstPlace.Add(player);
            }
        }
        public void AddFromFront(Player player) {
            if(firstPlace.Count < 1) {
                firstPlace.Add(player);
            } else if(secondPlace.Count < 1) {
                secondPlace.Add(player);
            } else if(thirdPlace.Count < 1) {
                thirdPlace.Add(player);
            } else if(fourthPlace.Count < 1) {
                fourthPlace.Add(player);
            }
        }
    }
}
