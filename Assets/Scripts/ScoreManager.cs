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

        public void Start()
        {
            UpdateDisplay();
        }

        public void UpdateDisplay()
        {
            team1text.text = "Team 1 Lives: " + team1lives;
            team2text.text = "Team 2 Lives: " + team2lives;
        }

        public void SubtractTeam1()
        {
            team1lives--;
            UpdateDisplay();
        }

        public void SubtractTeam2()
        {
            team2lives--;
            UpdateDisplay();
        }
    }
}
