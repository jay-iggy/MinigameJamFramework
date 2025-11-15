using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotatoBehavior : MonoBehaviour
{
    public float heatUpTime; // time for the potato to get hot
    public float heatUpTimeRandom; // random values in the range -heatUpTimeRandom, +heatUpTimeRandom are applied to heatUpTime each time the potato starts heating
    private float heatTimer = 0f;

    public Color hotColor; // the color the potato becomes when it is hot

    public ParticleSystem fireParticles;

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
    public void startHeating()
    {
        heatTimer = heatUpTime + Random.Range(-heatUpTimeRandom, heatUpTimeRandom);
    }

    // Update is called once per frame
    void Update()
    {
        // update potato heat timer
        if (heatTimer > 0) {
            heatTimer -= Time.deltaTime;

            // update potato color
            mat.color = Color.Lerp(hotColor, startColor, Mathf.Clamp((heatTimer / heatUpTime), 0, 1));

            // check if potato has primed yet
            if(heatTimer <= 0)
            {
                fireParticles.Play();
                Debug.Log("IM HOT");
            }
        }
    }
}
