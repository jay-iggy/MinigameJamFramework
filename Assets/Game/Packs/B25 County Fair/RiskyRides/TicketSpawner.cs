using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class TicketSpawner : MonoBehaviour
{
    [SerializeField]
    float minSpawnDelay = 0f;
    [SerializeField]
    float maxSpawnDelay = 4f;

    [SerializeField]
    GameObject ticket;

    [SerializeField]
    Transform[] spawnLocations;

    private void Start()
    {
        List<Transform> children = new List<Transform>();
        for (int i  = 0; i < transform.childCount; i++)
        {
            children.Add(transform.GetChild(i));
        }
        children.AddRange(spawnLocations);
        spawnLocations = children.ToArray();
        StartCoroutine(SpawnTicket());
    }

    private IEnumerator SpawnTicket()
    {
        yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
        Instantiate(ticket, spawnLocations[Random.Range(0, spawnLocations.Length)]);
        StartCoroutine(SpawnTicket());
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(255, 125, 0);
        foreach (Transform location in spawnLocations)
        {
            Gizmos.DrawSphere(location.position, 0.3f);
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            Gizmos.DrawSphere(transform.GetChild(i).position, 0.3f);
        }
    }
}
