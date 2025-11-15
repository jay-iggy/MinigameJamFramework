using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace HotPotatoGame {
    public enum Team
    {
        One,
        Two
    }
    public class TeamManager : MonoBehaviour
    {
        public ScarecrowPawn[] pawns;
        public GameObject teamIndicator1;
        public GameObject teamIndicator2;

        public void Start()
        {
            // randomize pawn teams with fisher-yates shuffle
            for (int i = pawns.Length - 1; i > 0; i--)
            {
                int j = Random.Range(0, i);
                ScarecrowPawn temp = pawns[i];
                pawns[i] = pawns[j];
                pawns[j] = temp;
            }
            // assign teams
            for (int i = 0; i < pawns.Length; i++)
            {
                Team t = (i % 2 == 0) ? Team.One : Team.Two;
                pawns[i].team = t;
                GameObject ti = Instantiate((t==Team.One) ? teamIndicator1 : teamIndicator2);
                ti.GetComponent<TeamIndicatorBehavior>().follow = pawns[i].transform;
            }
        }
    }
}
