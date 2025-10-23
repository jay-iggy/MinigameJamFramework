using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stencil : MonoBehaviour {
    [SerializeField] Transform pointsParent;
    private List<Vector3> _points = new();
    // Key: point position, Value: closest registered distance
    private Dictionary<Vector3, float> _pointDistances = new();
    [SerializeField] float scoreDistanceThreshold = 0.5f;
    
    private void Awake() {
        foreach (Transform child in pointsParent) {
            _points.Add(child.transform.position);
        }
    }
    
    private void EvaluatePoint(Vector3 key, Vector3 newPoint) {
        if (_pointDistances.ContainsKey(key)) {
            float existingDistance = _pointDistances[key];
            
            float newDistance = Vector3.Distance(newPoint, key);
            if (newDistance < existingDistance) {
                _pointDistances[key] = newDistance;
            }
        }
        else {
            float distance = Vector3.Distance(newPoint, key);
            _pointDistances.Add(key, distance);
        }
    }
    
    public void RegisterPoint(Vector3 position) {
        foreach (Transform child in pointsParent) {
            EvaluatePoint(child.position, position);
        }
    }

    public float CalculateScore() {
        float score = 0;

        foreach (KeyValuePair<Vector3, float> entry in _pointDistances) {
            if (entry.Value < scoreDistanceThreshold) {
                score++;
            }
        }

        print($"Score calculated: {score} / {_points.Count}, {_pointDistances.Count}");
        return score / _points.Count;
    }
    
}
