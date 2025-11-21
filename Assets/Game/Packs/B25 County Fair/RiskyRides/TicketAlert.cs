using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketAlert : MonoBehaviour
{
    [SerializeField]
    float time = 5.0f;

    [SerializeField]
    GameObject ticket;

    [SerializeField]
    GameObject soundPlayer;

    float startTime;

    private void Start()
    {
        startTime = Time.time;
    }

    private void FixedUpdate()
    {
        if (startTime + time < Time.time)
        {
            Instantiate(ticket, transform.position, Quaternion.identity);
            Instantiate(soundPlayer, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
