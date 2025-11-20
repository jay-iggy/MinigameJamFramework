using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircularMover : MonoBehaviour
{
    [SerializeField]
    Vector2 center;
    [SerializeField]
    float radius;
    [SerializeField]
    float degreesPerSecond;
    [SerializeField]
    float startingDegrees;

    float startTime;

    Rigidbody2D body;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        startTime = Time.time;
    }

    private void FixedUpdate()
    {
        float radians = (startingDegrees + ((Time.time - startTime) * degreesPerSecond)) * Mathf.PI / 180.0f;
        float xOffset = radius * Mathf.Cos(radians);
        float yOffset = radius * Mathf.Sin(radians);
        Vector2 target = new Vector2(center.x + xOffset, center.y + yOffset);
        body.velocity = (target - (Vector2) transform.position) / Time.fixedDeltaTime;
    }

    private void OnValidate()
    {
        float radians = (startingDegrees) * Mathf.PI / 180.0f;
        float xOffset = radius * Mathf.Cos(radians);
        float yOffset = radius * Mathf.Sin(radians);
        Vector2 target = new Vector2(center.x + xOffset, center.y + yOffset);
        transform.position = target;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(center, radius);
    }
}
