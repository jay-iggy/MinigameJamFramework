using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace HotPotatoGame {
    public class PotatoBehavior : MonoBehaviour
    {
        public float heatUpTime; // time for the potato to get hot
        public float heatUpTimeRandom; // random values in the range -heatUpTimeRandom, +heatUpTimeRandom are applied to heatUpTime each time the potato starts heating
        public float explodeUpTime;
        private float heatTimer = 0f;
        private float explodeTimer = 0f;
        public float explosionRadius;

        [SerializeField] private float initPulsingSpeed;
        private float currPulsingSpeed;
        private float phase = 0f;


        public Color hotColor; // the color the potato becomes when it is hot

        public LayerMask playerMask;

        public bool isOnFire = false;
        public ParticleSystem fireParticles;
        public GameObject explosion;
        public ParticleSystem explosionParticle;
        public AudioSource explosionAudio;


        private Color startColor;
        private Material mat;
        private Vector3 defaultMatScale;

        public GameObject recentThrower = null;



        private void Awake()
        {
            mat = GetComponent<MeshRenderer>().material;
            defaultMatScale = transform.localScale;
            startColor = mat.GetColor("_EmissionColor");
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
            mat.SetColor("_EmissionColor", startColor);
            heatTimer = heatUpTime + Random.Range(-heatUpTimeRandom, heatUpTimeRandom);
            explodeTimer = 0f;
            phase = 0f;
            currPulsingSpeed = initPulsingSpeed;
            transform.localScale = defaultMatScale;
        }

        // Update is called once per frame
        void Update()
        {

            //Check if on fire
            if(GetComponent<ThrowableBehavior>().isPickedUp && isOnFire)
            {
                explodeTimer += Time.deltaTime;

                if (explodeTimer >= explodeUpTime)
                {
                    GetComponent<ThrowableBehavior>().Drop();
                    blowUp();
                }
                else if (explodeTimer > explodeUpTime * 0.75)
                {
                    currPulsingSpeed = 20f;

                }
                else if (explodeTimer > explodeUpTime * 0.45)
                {
                    currPulsingSpeed = 10f;
                }
                else
                {
                    currPulsingSpeed = initPulsingSpeed;
                }

                phase += currPulsingSpeed * Time.deltaTime;
                float size = Mathf.Abs(Mathf.Sin(phase) / 2);
                transform.localScale = defaultMatScale + Vector3.one * size;
            }


            // update potato heat timer
            // only do so when the potato is held
            if (GetComponent<ThrowableBehavior>().isPickedUp && heatTimer > 0) {
                heatTimer -= Time.deltaTime;

                // update potato color
                mat.SetColor("_EmissionColor", Color.Lerp(hotColor, startColor, Mathf.Clamp((heatTimer / heatUpTime), 0, 1)));

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
                if(collision.gameObject != recentThrower) blowUp();
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

        private void blowUp()
        {
            recentThrower = null;
            // show and hide the explosion graphic
            StartCoroutine("ExplosionGraphic");
            explosionParticle.Play();

            // play sound
            explosionAudio.Play();

            // hit anyone in the blast zone
            Collider[] cols = Physics.OverlapSphere(explosion.transform.position, (explosionRadius), playerMask);
            foreach (Collider c in cols)
            {
                c.gameObject.GetComponent<DieBehavior>().Die();
            }

            // reset back to heating up again
            startHeating();
        }
    }
}
