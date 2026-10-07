using System.Collections;
using System.Collections.Generic;
using Examples.Splitscreen;
using UnityEngine;

namespace Examples.TrumbusTrace {
    public class Drill : MonoBehaviour {
        [SerializeField] LineRenderer lineRenderer;

        [SerializeField] float drillHeight = 0f;
        [SerializeField] float distancePerSegment = 0.5f;
        private Vector3 _lastPosition;

        [SerializeField] SubsceneManager subsceneManager;
        [SerializeField] Stencil stencil;

        public Color[] lineColor = { };

        public AudioSource scribbleSfx;
        private Rigidbody rb;

        void Awake() {
            rb = GetComponent<Rigidbody>();
        }

        void Start() {
            lineRenderer.SetPosition(0, new Vector3(transform.position.x, drillHeight, transform.position.z));
            Color color = lineColor[subsceneManager.playerIndex];
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
            _lastPosition = transform.position;
        }

        void Update() {
            // UPDATE DRAWING
            if (Vector3.Distance(_lastPosition, transform.position) >= distancePerSegment) {
                AddNewPoint(new Vector2(transform.position.x, transform.position.z));
                _lastPosition = transform.position;
            }
            // SFX
            if(scribbleSfx.isPlaying) {    
                if(rb.velocity.magnitude <= 0) {
                    scribbleSfx.Stop();
                }
            }
            else if (rb.velocity.magnitude > 0) {
                scribbleSfx.Play();
            }
            
        }

        void AddNewPoint(Vector2 newPoint) {
            int currentPointCount = lineRenderer.positionCount;
            lineRenderer.positionCount = currentPointCount + 1;
            lineRenderer.SetPosition(currentPointCount, new Vector3(newPoint.x, drillHeight, newPoint.y));
            stencil.RegisterDrawnPoint(new Vector3(newPoint.x, 0f, newPoint.y));
        }
    }
}
