using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace HotPotatoGame {
    public class ScoreManager : MonoBehaviour
    {
        public int team1lives;
        public int team2lives;

        public TextMeshProUGUI team1text;
        public TextMeshProUGUI team2text;

        public TextMeshProUGUI winsText;

        public ScarecrowPawn[] pawns;

        public void Start()
        {
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            team1text.text = "Team 1 Lives: " + team1lives;
            team2text.text = "Team 2 Lives: " + team2lives;
        }

        public void CheckForLoss()
        {
            if(team1lives <= 0)
            {
                StartCoroutine(EndSequence(Team.Two));
            }else if (team2lives <= 0){
                StartCoroutine(EndSequence(Team.One));
            }
        }

        public void EndMinigame(Team winner)
        {
            MinigameManager.Ranking _ranking = new();
            int[] rankingList = new int[] { 0, 0, 0, 0 };
            int index = 0;
            foreach(ScarecrowPawn p in pawns)
            {
                rankingList[index] = (p.team == winner) ? _ranking.GetNextHighestRank() : _ranking.GetNextLowestRank();
                index++;
            }

            _ranking.SetRanksFromList(rankingList);
            // TODO: Determine player rankings
            MinigameManager.instance.EndMinigame(_ranking);
        }

        public void SubtractTeam1()
        {
            team1lives--;
            CheckForLoss();
            UpdateDisplay();
        }

        public void SubtractTeam2()
        {
            team2lives--;
            CheckForLoss();
            UpdateDisplay();
        }

        public IEnumerator EndSequence(Team winner)
        {
            Time.timeScale = 0.1f;
            winsText.enabled = true;
            winsText.text = (winner == Team.One) ? "Team 1 Wins!" : "Team 2 Wins!";
            yield return new WaitForSeconds(0.5f);
            Time.timeScale = 1f;
            EndMinigame(winner);
            winsText.enabled = false;
            yield break;
        }
    }
}
