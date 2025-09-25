using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Examples.FumperFalls {
    public class ShrinkPlatform : MonoBehaviour {
        [SerializeField] AnimationCurve shrinkCurve;
        [SerializeField] FumperFallsGameManager gameManager;

        private Vector3 _defaultScale;

        private void Start() {
            _defaultScale = transform.localScale;
        }

        private void Update() {
            float scale = shrinkCurve.Evaluate(gameManager.timer / gameManager.duration);

            transform.localScale =
                new Vector3(_defaultScale.x * scale, transform.localScale.y, _defaultScale.z * scale);
        }
    }
}