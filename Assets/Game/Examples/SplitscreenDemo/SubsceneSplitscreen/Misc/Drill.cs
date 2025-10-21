using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Examples.Splitscreen.TrumbusTrace {
    public class Drill : MonoBehaviour {
        [SerializeField] LineRenderer lineRenderer;

        [SerializeField] float drillHeight = 0f;
        [SerializeField] float distancePerSegment = 0.5f;
        private Vector3 lastPosition;

        [SerializeField] SubsceneManager subsceneManager;

        public Color[] lineColor = { };

        void Start() {
            lineRenderer.SetPosition(0, new Vector3(transform.position.x, drillHeight, transform.position.z));
            Color color = lineColor[subsceneManager.playerIndex];
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }

        void Update() {
            if (Vector3.Distance(lastPosition, transform.position) >= distancePerSegment) {
                AddNewPoint(new Vector2(transform.position.x, transform.position.z));
                lastPosition = transform.position;
            }
        }

        void AddNewPoint(Vector2 newPoint) {
            int currentPointCount = lineRenderer.positionCount;
            lineRenderer.positionCount = currentPointCount + 1;
            lineRenderer.SetPosition(currentPointCount, new Vector3(newPoint.x, drillHeight, newPoint.y));
        }
    }
}
