using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ShooterMinigame {
    [RequireComponent(typeof(MeshRenderer))]
    public class Ball : MonoBehaviour {

        private ShooterMinigamePlayer m_owner;

        private bool scored = false;

        public ShooterMinigamePlayer Owner {
            get => m_owner;
            set {
                m_owner = value;
                m_renderer.material.color = value.Color;
            }
        }

        private MeshRenderer m_renderer => GetComponent<MeshRenderer>();

        private void OnCollisionEnter(Collision collision) {
            if (scored)
                return;

            if (collision.collider.gameObject.layer == 15)
                return;

            scored = true;

            if (collision.collider.gameObject.layer != 14)
                return;

            Target target = collision.collider.GetComponent<Target>();
            if (target == null)
                return;

            target.Scorable = false;
            Owner.Score();
        }
    }
}
