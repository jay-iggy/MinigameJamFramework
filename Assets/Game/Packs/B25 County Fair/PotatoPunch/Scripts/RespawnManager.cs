using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    public static RespawnManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void Respawn(GameObject obj, float respawnTime, GameObject objIndicator)
    {
        StartCoroutine(RespawnTimer(obj, respawnTime, objIndicator));
    }

    IEnumerator RespawnTimer(GameObject obj, float timer, GameObject objIndicator)
    {
        obj.SetActive(false);
        objIndicator.SetActive(false);
        yield return new WaitForSeconds(timer);
        obj.transform.position = new Vector3(0, 5f, 0);
        obj.SetActive(true);
        objIndicator.SetActive(true);
    }
}
