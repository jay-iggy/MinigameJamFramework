using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;
using UnityEngine.UIElements;
using PumpkinGhost;

public class GrowingPumpkin : MonoBehaviour
{

    public float maxSize;
    public float minSize;
    public float growRate;
    public float size = 1.0f;

    public int numP = 0;

    public GameObject pumpkinBody;
    private Renderer _pumpkinBodyRenderer;
    private float growRateModifier = 0;

    // Start is called before the first frame update
    void Start()
    {
        _pumpkinBodyRenderer = pumpkinBody.GetComponent<Renderer>();
        if (growRateModifier == 0)
        {
            growRateModifier = Random.Range(-0.04f, 0.04f);
        }
        if (numP > PlayerManager.GetNumPlayers()) {
            gameObject.SetActive(false);
            Destroy(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (size < maxSize){
            // Increase size if applicable
            size += (growRate + growRateModifier) * Time.deltaTime;

            // Set color based on size
            if (size < minSize)
            {
                _pumpkinBodyRenderer.material.SetColor("_Color", Color.HSVToRGB(0.08f + (0.17f * (1 - ((size - 0.6f) / 0.4f))), 0.95f, 0.9f, false));
            }

            //Enable/disable collider based on size
            if (size < minSize)
            {
                GetComponent<SphereCollider>().enabled = false;
            }
            else
            {
                GetComponent<SphereCollider>().enabled = true;
            }

            // Set size to max if it goes above
            if (size > maxSize)
            {
                size = maxSize;
            }

            // Set scale to factor by size
            transform.localScale = new Vector3(size, size, size);
            
            // Spawn back if outside of map.
            if (transform.position.x < -16) 
            {
                transform.position += Vector3.right * Random.Range(1,5);
            }
            else if (transform.position.x > 16) 
            {
                transform.position += Vector3.left * Random.Range(1,5);
            }

            if (transform.position.z < -16) 
            {
                transform.position += Vector3.forward * Random.Range(1,5);
            }
            else if (transform.position.z > 16) 
            {
                transform.position += Vector3.back * Random.Range(1,5);
            }
        }
    }

    // Collision detection for the trigger
    private void OnTriggerEnter(Collider other)
    {

        // Check if colliding with a player
        if (other.gameObject.CompareTag("Player") && size > minSize)
        {
            PumpkinGhost.PumpkinGhostPawn player = other.gameObject.GetComponent<PumpkinGhost.PumpkinGhostPawn>();
            player.pumpkinPickup = this;
        }
         else if (other.gameObject.CompareTag("MainCamera"))
        {
            size = 0.6f;
            
        }
    }

    // Player exiting
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PumpkinGhost.PumpkinGhostPawn player = other.gameObject.GetComponent<PumpkinGhost.PumpkinGhostPawn>();

            if (player.pumpkinPickup == this)
            {
                player.pumpkinPickup = null;
            }
            
        }
    }

    // Returns the size of the pumpkin
    public float GetSize() {
        return size;
    }

    public void Delete()
    {
        gameObject.SetActive(false);
        Destroy(this);
    }
}
