using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ShooterMinigame
{
    [RequireComponent(typeof(Collider))]
    public class Killbox : MonoBehaviour {
        private void OnTriggerEnter(Collider collider) {
            Destroy(collider.gameObject);
        }
    }
}