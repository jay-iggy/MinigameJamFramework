using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

namespace PumpkinGhost
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] public GameObject[] players = new GameObject[4];
        private bool follow = true;

        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            if(!follow) {
                return;
            }

            float x = 0;
            float z = 0;
            int count = 0;

            foreach (GameObject player in players) {
                if (player.activeSelf) {
                    count++;
                    x += player.transform.position.x;
                    z += player.transform.position.z;
                }
            }

            transform.position += (new Vector3(x / Math.Max(count, 1) * 0.8f, 15, z / Math.Max(count, 1) * 0.8f - 20) - transform.position) * Time.deltaTime;
        }
    }
}