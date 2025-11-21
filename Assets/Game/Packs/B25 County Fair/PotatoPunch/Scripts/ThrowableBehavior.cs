using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPotatoGame
{
    public class ThrowableBehavior : MonoBehaviour
    {
        public bool isPickedUp = false;

        protected Rigidbody rb;
        private Vector3 defaultScale;
        
        public float gravity = -10;

        private void Awake() {
            rb = GetComponent<Rigidbody>();
        }

        // Start is called before the first frame update
        void Start()
        {
            defaultScale = transform.localScale;
        }

        // Update is called once per frame
        void Update()
        {
            rb.AddForce(new Vector3 (0,gravity,0), ForceMode.Force);
        }

        // called when the player picks up a potato
        // hand is the transform that should hold the potato
        public virtual void PickUp(GameObject hand)
        {
            if (isPickedUp) return;
            isPickedUp = true;
            rb.isKinematic = true;
            transform.SetParent(hand.transform);
            transform.localPosition = Vector3.zero;
            GetComponent<Collider>().enabled = false;
        }

        // called when a player drops the potato
        public virtual void Drop()
        {
            if (!isPickedUp) return;
            isPickedUp = false;
            transform.SetParent(null);
            rb.isKinematic = false;
            GetComponent<Collider>().enabled = true;
            transform.localScale = defaultScale;
        }
    }
}
