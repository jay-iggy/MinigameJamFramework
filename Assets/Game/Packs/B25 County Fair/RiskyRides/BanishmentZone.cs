using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BanishmentZone : MonoBehaviour
{
    [SerializeField]
    GameObject banishmentEffect;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Instantiate(banishmentEffect, collision.transform.position, Quaternion.identity);
        collision.transform.position = new Vector3(0, -1000000, 0);
    }
}
