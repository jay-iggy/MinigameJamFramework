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
        public float postPunchCooldown = 0.5f; // amount of time after punch until player can pick up potato

        public void Update()
        {
            // decrement timer
            if (inactiveTimer > 0)
            {
                inactiveTimer -= Time.deltaTime;
            }
        }
        private void OnTriggerEnter(Collider collider)
        {

            if (inactiveTimer <= 0 && ((potatoMask & (1 << collider.gameObject.layer)) != 0))
            {
                PotatoBehavior potato = collider.gameObject.GetComponent<PotatoBehavior>();
                if (potato.isPickedUp) return;
                pawn.holdingPotato = true;
                potato.PickUp(hand);
            }
        }

        public void Drop()
        {
            if (hand.GetComponentInChildren<PotatoBehavior>() != null)
            {
                pawn.holdingPotato = false;
                inactiveTimer = postPunchCooldown;
                hand.GetComponentInChildren<PotatoBehavior>().Drop();
            }
        }
    }
}
