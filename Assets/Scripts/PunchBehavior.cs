using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchBehavior : MonoBehaviour
{
    public float punchForce;
    public float punchLift;
    public BoxCollider scanBox;
    public LayerMask playerMask;

    private Rigidbody rb;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void Punch()
    {
        // scan for any colliders in the punch region
        Collider[] hitColliders = Physics.OverlapBox((scanBox.transform.position), (0.5f * scanBox.bounds.extents), Quaternion.identity, playerMask);
        // apply an impulse to every collider
        foreach(Collider col in hitColliders)
        {
            // calculate the direction to apply the force (away from the punching player)
            float rot = (rb.rotation.eulerAngles.y);
            Vector3 direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * rot), punchLift, Mathf.Cos(Mathf.Deg2Rad * rot));
            // add the impulse
            col.gameObject.GetComponent<Rigidbody>().AddForce(direction * punchForce, ForceMode.Impulse);
            // make the player drop the potato
            col.gameObject.GetComponentInChildren<CatchBehavior>().Drop(); // TODO: this reference is very brittle and could cause issues
        }
    }
}
