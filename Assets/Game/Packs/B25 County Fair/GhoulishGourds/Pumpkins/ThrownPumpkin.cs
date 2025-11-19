using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using PumpkinGhost;
using UnityEngine;

public class ThrownPumpkin : MonoBehaviour
{

    public float speed;
    public GameObject model;
    public int playerNum;
    public GameObject pumpkinRespawn;
    public GameObject particles;

    //Sound Effects:
    [SerializeField] private AudioClip sound_pumpkinHit;
    private AudioSource _audio;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody>().AddForce(transform.forward * speed, ForceMode.Force);
        _audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        model.transform.Rotate(0, speed * Time.deltaTime, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if colliding with a balloon
        if (other.gameObject.CompareTag("Respawn"))
        {
            other.gameObject.transform.GetComponent<Balloon>().PumpkinHitBalloon(playerNum, this.transform.localScale.x);
            // Place 1-3 new pumpkins on the ground
            int numPumpkins = (int)Mathf.Ceil(UnityEngine.Random.Range(2, 5) / 2);
            for (int i = 0; i < numPumpkins; i++)
            {
                // Places a pumpkin
                do {
                    pumpkinRespawn.transform.position = transform.position + new Vector3(UnityEngine.Random.Range(-3.0f, 3.0f), 0, UnityEngine.Random.Range(-3.0f, 3.0f));
                } while (Math.Abs(pumpkinRespawn.transform.position.x) > 27 || Math.Abs(pumpkinRespawn.transform.position.z) > 27);
                Instantiate(pumpkinRespawn);
            }

            _audio.PlayOneShot(sound_pumpkinHit);

            StartCoroutine(DestroySelf());
        }
        else if (other.gameObject.CompareTag("Finish") || other.gameObject.CompareTag("MainCamera"))
        {
            // if it is a held pumpkin
            if (other.gameObject && other.gameObject.transform && other.gameObject.transform.parent && other.gameObject.transform.parent.CompareTag("Player")) {
                other.gameObject.transform.parent.GetComponent<PumpkinGhostPawn>().BreakHeldPumpkin();
            }

            pumpkinRespawn.transform.position = transform.position;
            Instantiate(pumpkinRespawn);
            
            _audio.PlayOneShot(sound_pumpkinHit);
            
            StartCoroutine(DestroySelf());
        }


    }

    IEnumerator DestroySelf() {

        particles.transform.position = transform.position;
        Instantiate(particles);

        speed = 0;
        this.transform.localScale = new Vector3(0,0,0);
        this.GetComponent<Collider>().enabled = false;

        yield return new WaitForSeconds(sound_pumpkinHit.length);

        Destroy(this);
        gameObject.SetActive(false);
    }
}
