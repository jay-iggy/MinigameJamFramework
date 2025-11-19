using System.Collections.Generic;
using UnityEngine;

namespace Starter.PumpkinPlunge {
    public class FloatyWater : MonoBehaviour {
        [Header("Properties")]
        public Collider playerCollider;
        private Rigidbody playerBody => playerCollider.GetComponent<Rigidbody>();
        public float waterLevel = 0f;
        public float floatHeight = 0.6f;
        public float buoyantForce = 1f;
        public float waterDrag = 0.95f;
        public float waterAngularDrag = 0.5f;
        private bool inWater = false;

        void OnTriggerEnter(Collider other)
        {
            if (other == playerCollider)
            {
                inWater = true;
            }
        }

        void OnTriggerExit(Collider other)
        {
            if (other == playerCollider)
            {
                inWater = false;
            }
        }

        void FixedUpdate()
        {
            if (inWater)
            {
                float displacementMult = Mathf.Clamp01((waterLevel - playerBody.position.y) / floatHeight);
                Vector3 lift = -Physics.gravity * displacementMult * buoyantForce;
                playerBody.AddForce(lift, ForceMode.Acceleration);

                playerBody.drag = waterDrag;
                playerBody.angularDrag = waterAngularDrag;
            }
            else if (playerBody.drag != 0f || playerBody.angularDrag != 0f)
            {
                playerBody.drag = 0f;
                playerBody.angularDrag = 0f;
            }
        }
    }
}