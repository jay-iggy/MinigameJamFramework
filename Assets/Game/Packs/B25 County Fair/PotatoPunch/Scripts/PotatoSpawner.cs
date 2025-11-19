using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPotatoGame
{
    public class PotatoSpawner : MonoBehaviour
    {
        public Transform topLeft;
        public Transform botRight;
        public GameObject potato;

        public float timeToStart;
        public float timeToMin;
        public float startDelay;
        public float minDelay;

        private float timer = 0;
        private float spawnTimer = 0;
        private bool started = false;

        // Start is called before the first frame update
        void Start()
        {
            timer = timeToStart;
        }

        // Update is called once per frame
        void Update()
        {
            if (timer > 0)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    if (!started)
                    {
                        started = true;
                        spawnTimer = startDelay;
                        timer = timeToMin;
                        return;
                    }
                }
            }

            if (spawnTimer > 0)
            {
                spawnTimer -= Time.deltaTime;
                if (spawnTimer <= 0)
                {
                    Spawn();
                    spawnTimer = Mathf.Lerp(minDelay, startDelay, (timer / timeToMin));
                }
            }
        }

        void Spawn()
        {
            Instantiate(potato,
                new Vector3(Random.Range(topLeft.position.x, botRight.position.x),
                transform.position.y,
                Random.Range(topLeft.position.z, botRight.position.z)),
                Quaternion.identity);
        }
    }
}
