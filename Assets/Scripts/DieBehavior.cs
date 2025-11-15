using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPotatoGame
{
    public class DieBehavior : MonoBehaviour
    {
        private Team team;
        private ScoreManager sm;

        public void Awake()
        {
            team = GetComponent<ScarecrowPawn>().team;
            sm = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        }

        public void Die()
        {
            if(team == Team.One)
            {
                sm.SubtractTeam1();
            }else if(team == Team.Two)
            {
                sm.SubtractTeam2();
            }
        }
    }
}
