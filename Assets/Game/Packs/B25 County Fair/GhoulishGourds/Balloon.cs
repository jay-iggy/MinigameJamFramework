using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PumpkinGhost {
    public class Balloon : MonoBehaviour
    {
        private Vector3 tip;
        public MonoBehaviour owner;
        public float balloonLength = 1.0F;
        private float tiltX = 45.0F;
        public float tiltY = 0.0F;
        private float accY = 0.0F;
        public float vY = 0.0F;
        private float rigidity = 54.2F;
        private float prevY;
        private Vector3 trailPos;
        private float angleLimit = 150.0F;

        private float killable = 1.0F;
        private float penalty = 0.0F;

        private GameObject manager;


        [SerializeField] private AudioClip sound_balloonPop;
        private AudioSource _audio;

        // Start is called before the first frame update
        void Start()
        {
            manager = GameObject.Find("PumpkinGhostManager");
            prevY = owner.transform.rotation.eulerAngles.y;
            //trailPos = owner.transform.position + (owner.transform.rotation * new Vector3(0,0,-1.8F));

            _audio = GetComponent<AudioSource>();
        }

        void set_owner(MonoBehaviour o){
            owner = o;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 goalPos = owner.transform.position + (owner.transform.rotation * new Vector3(0,0,-0.95F));
            //Vector3 d = trailPos - owner.transform.position;
            //trailPos = owner.transform.position + d / d.magnitude * 1.8F;
            transform.position = goalPos; //owner.transform.position + new Vector3(1,0,0);
            //Vector3 d = tip - goalPos;
            //if (d.magnitude > balloonLength){
            //    tip = goalPos + d / d.magnitude * balloonLength;
            //}
            //tip = goalPos + owner.transform.rotation * new Vector3(0,1,-1);
            //Quaternion idealRot = Quaternion.LookRotation(owner.transform.rotation * new Vector3(0,1,-0.5F),owner.transform.rotation * new Vector3(0,0.5F,-1));
            //float prevY = transform.rotation.y - owner.transform.rotation.y;
            //if (prevY != -666.0F){
            
            //tiltY += owner.transform.rotation.eulerAngles.y - transform.rotation.eulerAngles.y;
            
            float lol = -Mathf.DeltaAngle(tiltY, owner.transform.rotation.eulerAngles.y + 180);
            if (Mathf.Abs(lol) > angleLimit){
                tiltY = owner.transform.rotation.eulerAngles.y + angleLimit * Mathf.Sign(lol);
                lol = Mathf.Sign(lol) * angleLimit;
                //vY = 800.0F * Mathf.Sign(lol); 

            }
            vY += Mathf.DeltaAngle(prevY, owner.transform.rotation.eulerAngles.y) * 1.4F;
            accY = -rigidity * lol;
            vY += accY * Time.deltaTime;
            vY *= Mathf.Pow(0.34F, Time.deltaTime);
            tiltY += vY * Time.deltaTime;
            //tiltY = Vector3.Angle(goalPos - owner.transform.position, trailPos - owner.transform.position);

            transform.rotation = Quaternion.Euler(tiltX + Mathf.Sin(penalty * 48.0F) * 14.0F,tiltY,0);

            if (killable < 1.0F){
                killable += 2.0F * Time.deltaTime;
                killable = Mathf.Min(killable, 1.0F);
                transform.localScale = new Vector3(1,1,1) * Mathf.Max(killable,0.0F);
            }
            else{
                Vector3 pPos = goalPos + (owner.transform.rotation * new Vector3(0,0,-0.55F));
                if (pPos.x >= 16 || pPos.x <= -16 || pPos.z >= 15 || pPos.z <= -16.5){
                    penalty += Time.deltaTime;
                    if (penalty >= 2.0F){
                        manager.GetComponent<PumpkinGhostGameManager>().AddScore(owner.GetComponent<PumpkinGhostPawn>().get_number(), 0.0f);
                        killable = -1.0F;
                        penalty = 0.0F;
                        _audio.PlayOneShot(sound_balloonPop);
                    }
                }
                else{
                    penalty = 0.0F;
                }
                transform.localScale = new Vector3(1.0F + penalty,1.0F + penalty,1.0F + penalty);
            }

            prevY = owner.transform.rotation.eulerAngles.y;

            //tiltX += 5.0F;

            //Quaternion.RotateTowards(transform.rotation, target.rotation, step);
        }

        public void PumpkinHitBalloon(int playerNum, float hitPumpkinSize)
        {
            if (killable >= 1.0F)
            {
                manager.GetComponent<PumpkinGhostGameManager>().AddScore(playerNum, hitPumpkinSize);
                manager.GetComponent<PumpkinGhostGameManager>().AddScore(owner.GetComponent<PumpkinGhostPawn>().playerNum, 0.5f);
                killable = -1.0F;
                _audio.PlayOneShot(sound_balloonPop);
            }
        }
    }
}