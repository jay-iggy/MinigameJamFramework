using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudShadowMoving : MonoBehaviour
{
    public float speed = 1f;          
    public float resetX = -15f;       
    public float endX = 15f;          

    private void Update()
    {
        
        transform.position += Vector3.right * speed * Time.deltaTime;

        
        if (speed > 0 && transform.position.x > endX)
        {
            transform.position = new Vector3(resetX, transform.position.y, transform.position.z);
        }
        else if (speed < 0 && transform.position.x < endX)
        {
            transform.position = new Vector3(resetX, transform.position.y, transform.position.z);
        }
    }
}
