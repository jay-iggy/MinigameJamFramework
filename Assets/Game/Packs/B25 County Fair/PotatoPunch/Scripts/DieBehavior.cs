using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPotatoGame
{
    public class DieBehavior : MonoBehaviour
    {
        private ScoreManager sm;
        [SerializeField] private float respawnTime;
        private ScarecrowPawn sp;

        public void Start()
        {
            sm = GameObject.Find("ScoreManager").GetComponent<ScoreManager>();
            sp = GetComponent<ScarecrowPawn>();
        }

        public void Die()
        {
            GetComponentInChildren<CatchBehavior>().Drop();
            if (GetComponent<ScarecrowPawn>().team == Team.One)
            {
                sm.SubtractTeam1();
            }else if(GetComponent<ScarecrowPawn>().team == Team.Two)
            {
                sm.SubtractTeam2();
            }

            RespawnManager.instance.Respawn(this.gameObject, respawnTime, getCorrespondingIndicator());
        }

        private GameObject getCorrespondingIndicator()
        {
            foreach (Transform indicator in TeamManager.instance.transform)
            {
                int num = indicator.GetComponent<TeamIndicatorBehavior>().playerNum;

                //Is Found!
                if (sp.playerNum == num)
                {
                    return indicator.gameObject;
                }
            }

            Debug.LogError("Bug in finding Team Indicator!");
            return null;
        }
    }
}
