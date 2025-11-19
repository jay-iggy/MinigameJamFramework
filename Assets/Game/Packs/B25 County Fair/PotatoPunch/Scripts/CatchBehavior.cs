using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPotatoGame
{
    public class CatchBehavior : MonoBehaviour
    {
        public ScarecrowPawn pawn;
        public GameObject hand;
        public LayerMask potatoMask;
        private float inactiveTimer = 0; // if > 0, player cannot pick up the potato
        public float postPunchCooldown = 0.2f; // amount of time after punch until player can pick up potato

        public void Update()
        {
            if(GetComponent<Collider>().enabled == false)
            {
                GetComponent<Collider>().enabled = true;
            }

            // decrement timer
            if (inactiveTimer > 0)
            {
                inactiveTimer -= Time.deltaTime;
                if(inactiveTimer <= 0)
                {
                    GetComponent<Collider>().enabled = false;
                }
            }
        }
        private void OnTriggerEnter(Collider collider)
        {

            if (inactiveTimer <= 0 && ((potatoMask & (1 << collider.gameObject.layer)) != 0))
            {
                ThrowableBehavior potato = collider.gameObject.GetComponent<ThrowableBehavior>();
                if (potato.isPickedUp) return;
                if (collider.gameObject.GetComponent<PotatoBehavior>().isOnFire) return;
                pawn.holdingPotato = true;
                potato.PickUp(hand);
            }
        }

        public void Drop()
        {
            if (hand.GetComponentInChildren<ThrowableBehavior>() != null)
            {
                pawn.holdingPotato = false;
                inactiveTimer = postPunchCooldown;
                hand.GetComponentInChildren<ThrowableBehavior>().Drop();
            }
        }
    }
}
