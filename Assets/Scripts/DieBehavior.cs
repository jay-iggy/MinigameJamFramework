using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPotatoGame
{
    public class DieBehavior : MonoBehaviour
    {
        private ScoreManager sm;

        public void Start()
        {
            sm = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
        }

        public void Die()
        {
            if (GetComponent<ScarecrowPawn>().team == Team.One)
            {
                sm.SubtractTeam1();
            }else if(GetComponent<ScarecrowPawn>().team == Team.Two)
            {
                sm.SubtractTeam2();
            }
        }
    }
}
