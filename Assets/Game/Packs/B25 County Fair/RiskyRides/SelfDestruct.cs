using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    public float time;

    void Start()
    {
        StartCoroutine(Kill());
    }

    IEnumerator Kill()
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
