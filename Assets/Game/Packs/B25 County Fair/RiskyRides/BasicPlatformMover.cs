using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicPlatformMover : MonoBehaviour
{
    Rigidbody2D body;

    public float distance = 5;
    public float distance2 = 1;

    public float speed = 0.5f;
    public float speed2 = 0.3f;

    float offset = 0;
    float offset2 = 0;

    Vector3 reference;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        reference = transform.position;
        offset = Random.Range(0, 2 * Mathf.PI);
        offset2 = Random.Range(0, 2 * Mathf.PI);
    }

    void FixedUpdate()
    {
        transform.position = reference + new Vector3(distance * Mathf.Sin(Time.time * speed + offset), distance2 * Mathf.Sin(Time.time * speed2 + offset2));
    }
}
