using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticket : MonoBehaviour
{
    [SerializeField]
    GameObject soundPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        FindObjectOfType<RiskyRidesGameManager>().GivePoint(collision.GetComponent<RiskRidePawn>().team);
        Instantiate(soundPlayer, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
