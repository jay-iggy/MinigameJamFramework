using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Starter.PumpkinPlunge {
    public class Trapdoor : MonoBehaviour {
        [Header("Properties")]
        public TrapdoorType type;
        
        [Header("References")]
        public GameObject latch;
        public GameObject indicator;
        public GameObject indicatorSymbol;
        public GameObject bar;

        // Constants
        private static float LATCH_ROTATION = 90;
        private static float LATCH_OPEN_MASS = 0.5f;
        private static float LATCH_OPEN_FORCE = 500;

        void Start()
        {
            UpdateType();
        }

        void UpdateType()
        {
            latch.GetComponent<MeshRenderer>().material = type.material;
            indicator.GetComponent<MeshRenderer>().material = type.material;
            bar.GetComponent<MeshRenderer>().material = type.material;
            SpriteRenderer symbolRenderer = indicatorSymbol.GetComponent<SpriteRenderer>();
            symbolRenderer.sprite = type.symbol;
            symbolRenderer.color = type.symbolColor;
            indicatorSymbol.transform.localPosition += new Vector3(0, 0, type.symbolOffset);
        }

        public void OpenLatch()
        {
            Rigidbody latchBody = latch.GetComponent<Rigidbody>();
            latchBody.mass = LATCH_OPEN_MASS;
            HingeJoint hinge = latch.GetComponent<HingeJoint>();
            JointLimits limits = hinge.limits;
            limits.max = LATCH_ROTATION;
            hinge.limits = limits;
            latchBody.AddForce(-LATCH_OPEN_FORCE * Vector3.up);
        }
    }
}