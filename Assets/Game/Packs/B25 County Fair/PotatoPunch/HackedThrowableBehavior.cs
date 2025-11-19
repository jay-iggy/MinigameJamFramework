using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HotPotatoGame
{
    public class HackedThrowableBehavior : ThrowableBehavior
    {
        public void Start()
        {
            isPickedUp = true;
        }

        // called when the player picks up a potato
        // hand is the transform that should hold the potato
        public override void PickUp(GameObject hand)
        {
            return;
        }

        // called when a player drops the potato
        public override void Drop()
        {
            return;
        }
    }
}
