using Game.Examples;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using TMPro;
using UnityEngine;

public class RiskyRidesGameManager : MonoBehaviour
{
    public enum Team
    {
        RED = 0,
        BLUE = 1,
    }

    [SerializeField]
    byte redScore;
    [SerializeField]
    byte blueScore;

    [SerializeField]
    byte winScore = 10;

    [SerializeField]
    List<RiskRidePawn> pawns;

    [SerializeField]
    float gameTime = 60;

    [SerializeField]
    TextMeshProUGUI timerText;
    [SerializeField]
    TextMeshProUGUI redScoreText;
    [SerializeField]
    TextMeshProUGUI blueScoreText;

    private void Start()
    {
        AssignTeams();
    }

    private void Update()
    {
        gameTime -= Time.deltaTime;
        timerText.text = Mathf.CeilToInt(gameTime).ToString();
        if (gameTime <= 0)
        {

            EndGame();
        }
    }

    private void FixedUpdate()
    {
        bool aliveFlag = false;
        foreach (RiskRidePawn pawn in pawns)
        {
            if (pawn.transform.position.y > -15.0f) 
                aliveFlag = true;
        }
        if (!aliveFlag)
            EndGame();
    }

    public void AssignTeams()
    {
        //List<Player> players = PlayerManager.players;
        //players.Sort(delegate (Player p1, Player p2)
        //{
        //    if (p1.points == p2.points) 
        //        return 0;
        //    else 
        //        return (p2.points > p1.points) ? -1 : 1;
        //});
        //for (int i = 0; i < players.Count; ++i)
        //{
        //    pawns[players[i].playerIndex].team = (i == 0 || i == 3) ? Team.BLUE : Team.RED;
        //    pawns[players[i].playerIndex].UpdatePlayer();
        //}
        //for (int i = players.Count; i < pawns.Count; i++)
        //{
        //    //Destroy(pawns[i].gameObject);
        //}
    }

    public void GivePoint(Team team)
    {
        switch (team)
        {
            case Team.RED:
                redScore++;
                redScoreText.text = redScore.ToString();
                if (redScore >= winScore) 
                    EndGame();
                break;
            case Team.BLUE:
                blueScore++;
                blueScoreText.text = blueScore.ToString();
                if (blueScore >= winScore) 
                    EndGame();
                break;
            default:
                break;
        }
    }

    private void EndGame()
    {
        MinigameManager.Ranking rankings = new MinigameManager.Ranking();

        List<int> scores = new()
        {
            (pawns[0].team == Team.RED) ? redScore : blueScore,
            (pawns[1].team == Team.RED) ? redScore : blueScore,
            (pawns[2].team == Team.RED) ? redScore : blueScore,
            (pawns[3].team == Team.RED) ? redScore : blueScore
        };

        rankings.DetermineRankingFromScores(scores);

        EndMinigame(rankings);
    }

    public void EndMinigame(MinigameManager.Ranking ranking)
    {
        // TODO: Determine player rankings
        MinigameManager.instance.EndMinigame(ranking);
    }
}
