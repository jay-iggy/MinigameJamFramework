using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField]
    float degreesPerSecond = 90f;

    [SerializeField]
    float startingDegrees = 0f;

    float startTime;

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        transform.rotation = Quaternion.Euler(0, 0, startingDegrees + degreesPerSecond * (Time.time - startTime));
    }
}
