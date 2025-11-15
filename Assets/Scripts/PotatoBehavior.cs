using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPotatoGame {
    public class PotatoBehavior : MonoBehaviour
    {
        public float heatUpTime; // time for the potato to get hot
        public float heatUpTimeRandom; // random values in the range -heatUpTimeRandom, +heatUpTimeRandom are applied to heatUpTime each time the potato starts heating
        private float heatTimer = 0f;


        public Color hotColor; // the color the potato becomes when it is hot

        public LayerMask playerMask;

        public bool isOnFire = false;
        public ParticleSystem fireParticles;
        public GameObject explosion;


        private Color startColor;
        private Material mat;

        private void Awake()
        {
            mat = GetComponent<MeshRenderer>().material;
            startColor = mat.color;
        }

        // Start is called before the first frame update
        void Start()
        {
            startHeating();
        }

        // begins the potato heat up cycle
        // resets everything from previous cycles
        public void startHeating()
        {
            isOnFire = false;
            fireParticles.Stop();
            mat.color = startColor;
            heatTimer = heatUpTime + Random.Range(-heatUpTimeRandom, heatUpTimeRandom);
        }

        // Update is called once per frame
        void Update()
        {
            // update potato heat timer
            // only do so when the potato is held
            if (GetComponent<ThrowableBehavior>().isPickedUp && heatTimer > 0) {
                heatTimer -= Time.deltaTime;

                // update potato color
                mat.color = Color.Lerp(hotColor, startColor, Mathf.Clamp((heatTimer / heatUpTime), 0, 1));

                // check if potato has primed yet
                if (heatTimer <= 0)
                {
                    fireParticles.Play();
                    isOnFire = true;
                }
            }
        }

        // trigger the explosion
        private void OnCollisionEnter(Collision collision)
        {
            if (isOnFire)
            {
                // show and hide the explosion graphic
                StartCoroutine("ExplosionGraphic");

                // hit anyone in the blast zone
                Collider[] cols = Physics.OverlapSphere(explosion.transform.position, (0.5f * explosion.transform.lossyScale.x), playerMask);
                foreach(Collider c in cols)
                {
                    c.gameObject.GetComponent<DieBehavior>().Die();
                }

                // reset back to heating up again
                startHeating();
            }
        }

        public IEnumerator ExplosionGraphic()
        {
            explosion.GetComponent<MeshRenderer>().enabled = true;
            explosion.transform.SetParent(null);
            yield return new WaitForSeconds(0.3f);
            explosion.GetComponent<MeshRenderer>().enabled = false;
            explosion.transform.SetParent(transform);
            explosion.transform.localPosition = Vector3.zero;
            yield break;
        }
    }
}
