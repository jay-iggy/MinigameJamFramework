using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StencilRotation : MonoBehaviour {
    
    [SerializeField] List<float> possibleRotations = new List<float>();
    
    void Start() {
        int randomIndex = Random.Range(0, possibleRotations.Count);
        float chosenRotation = possibleRotations[randomIndex];
        transform.eulerAngles = new Vector3(90, 0,chosenRotation);
    }
}
