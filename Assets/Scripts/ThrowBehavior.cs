using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPotatoGame
{
    public class ThrowBehavior : MonoBehaviour
    {
        public float throwForce;
        public GameObject hand;

        private Rigidbody rb;

        public void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }
        public void Throw()
        {
            ThrowableBehavior potato = hand.GetComponentInChildren<ThrowableBehavior>();
            if (potato == null) return;
            // calculate the direction to apply the force (away from the punching player)
            float rot = (rb.rotation.eulerAngles.y);
            Vector3 direction = new Vector3(Mathf.Sin(Mathf.Deg2Rad * rot), 0f, Mathf.Cos(Mathf.Deg2Rad * rot));
            // drop the potato
            GetComponentInChildren<CatchBehavior>().Drop();
            // apply force to the potato to throw it
            potato.gameObject.GetComponent<Rigidbody>().AddForce(direction * throwForce, ForceMode.Impulse);
        }
    }
}
