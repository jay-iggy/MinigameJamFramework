using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPotatoGame
{
    public class ThrowableBehavior : MonoBehaviour
    {
        public bool isPickedUp = false;

        private Rigidbody rb;

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        // called when the player picks up a potato
        // hand is the transform that should hold the potato
        public void PickUp(GameObject hand)
        {
            if (isPickedUp) return;
            isPickedUp = true;
            rb.isKinematic = true;
            transform.SetParent(hand.transform);
            transform.localPosition = Vector3.zero;
        }

        // called when a player drops the potato
        public void Drop()
        {
            if (!isPickedUp) return;
            isPickedUp = false;
            transform.SetParent(null);
            rb.isKinematic = false;
        }
    }
}
