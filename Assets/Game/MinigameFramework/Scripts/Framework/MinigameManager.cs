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
    
    public SceneField resultsScene;
    
    [Header("Points")]
    public int pointsForFirstPlace = 4;
    public int pointsForSecondPlace = 3;
    public int pointsForThirdPlace = 2;
    public int pointsForFourthPlace = 1;
    
    [Header("Minigames")]
    public List<MinigameInfo> minigames;
    
    public void LoadRandomMinigame() {
        int randomIndex = UnityEngine.Random.Range(0, minigames.Count);
        LoadMinigame(minigames[randomIndex]);
    }
    
    public void LoadMinigame(MinigameInfo minigame) {
        SceneManager.LoadScene(minigame.scene.SceneName);
        PlayerManager.SetMinigameActionMap();
    }
    
    public void EndMinigame(ResultsRanking ranking) {
        AwardPoints(ranking);
        SceneManager.LoadScene(resultsScene.SceneName);
        PlayerManager.SetMenuActionMap();
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
    public struct ResultsRanking {
        public List<Player> firstPlace;
        public List<Player> secondPlace;
        public List<Player> thirdPlace;
        public List<Player> fourthPlace;
    }
}
