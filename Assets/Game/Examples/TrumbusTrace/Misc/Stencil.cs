using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Examples.TrumbusTrace {
    public class Stencil : MonoBehaviour {
        [SerializeField] Transform keyPointsParent;

        private List<Vector3> _keyPoints = new();

        // Key: point position, Value: closest registered distance
        private Dictionary<Vector3, float> _pointDistances = new();
        [SerializeField] float scoreDistanceThreshold = 0.5f;
        [SerializeField] List<float> possibleRotations = new List<float>();

        private void Awake() {
            SetRotation();
            foreach (Transform child in keyPointsParent) {
                _keyPoints.Add(child.transform.position);
            }
        }

        private void SetRotation() {
            int randomIndex = Random.Range(0, possibleRotations.Count);
            float chosenRotation = possibleRotations[randomIndex];
            transform.eulerAngles = new Vector3(0, chosenRotation, 0);
        }

        public void RegisterDrawnPoint(Vector3 position) {
            foreach (Vector3 key in _keyPoints) {
                EvaluateDistanceToKeyPoint(key, position);
            }
        }

        private void EvaluateDistanceToKeyPoint(Vector3 key, Vector3 drawnPoint) {
            if (_pointDistances.ContainsKey(key)) {
                float existingDistance = _pointDistances[key];

                float newDistance = Vector3.Distance(drawnPoint, key);
                if (newDistance < existingDistance) {
                    _pointDistances[key] = newDistance;
                }
            }
            else {
                float distance = Vector3.Distance(drawnPoint, key);
                _pointDistances.Add(key, distance);
            }
        }

        public float CalculateScore() {
            float score = 0;
            foreach (KeyValuePair<Vector3, float> entry in _pointDistances) {
                if (entry.Value < scoreDistanceThreshold) {
                    score++;
                }
            }

            return Mathf.RoundToInt((score / _keyPoints.Count)*100);
        }
    }
}
