using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;


namespace HotPotatoGame {
    public enum Team
    {
        One,
        Two
    }
    public class TeamManager : MonoBehaviour
    {
        public ScarecrowPawn[] pawns;
        public GameObject teamIndicator;

        public Material[] playerMats;

        public Color[] playerColors;


        public static TeamManager instance;

        private void Awake()
        {
            instance = this;
        }

        public void Start()
        {
            Team[] teams = new Team[] { Team.One, Team.One, Team.Two, Team.Two };
            // randomize pawn teams with fisher-yates shuffle
            for (int i = teams.Length - 1; i > 0; i--)
            {
                int j = Random.Range(0, i);
                Team temp = teams[i];
                teams[i] = teams[j];
                teams[j] = temp;
            }
            // assign teams
            for (int i = 0; i < pawns.Length; i++)
            {
                Team t = teams[i];
                pawns[i].team = t;
                GameObject ti = Instantiate(teamIndicator);
                TextMeshPro text = ti.GetComponentInChildren<TextMeshPro>();
                text.text = (t == Team.One) ? "T1" : "T2";
                text.fontSharedMaterial = playerMats[i];
                text.color = playerColors[i];
                text.ForceMeshUpdate();
                ti.GetComponent<TeamIndicatorBehavior>().follow = pawns[i].transform;
                ti.transform.SetParent(this.transform);
                ti.GetComponent<TeamIndicatorBehavior>().playerNum = pawns[i].playerNum;
            }
        }
    }
}
