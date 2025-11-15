using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamIndicatorBehavior : MonoBehaviour
{
    public Vector3 offset;
    public Transform follow;
    public int playerNum;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = follow.position + offset;  
    }
}
