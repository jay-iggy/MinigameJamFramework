using System;
using System.Collections;
using System.Collections.Generic;
using Examples.Splitscreen;
using TMPro;
using UnityEngine;

namespace Examples.TrumbusTrace {
    public class TraceSubmanager : MonoBehaviour {
        public int playerIndex { get; private set; }
        [SerializeField] TextMeshProUGUI scoreText;
        [SerializeField] private Stencil stencil;
        [SerializeField] MeshRenderer playerColorRenderer;

        [SerializeField] List<Material> materials = new();
        [SerializeField] private Color[] textColors;

        private void Awake() {
            playerIndex = TrumbusTraceManager.instance.subscenes.Count;
            TrumbusTraceManager.instance.subscenes.Add(this);
        }

        private void Start() {
            playerColorRenderer.material = materials[playerIndex];
        }

        public float CalculateAndDisplayScore() {
            float score = stencil.CalculateScore();
            StartCoroutine(AnimateScoreText(score));
            return score;
        }

        private IEnumerator AnimateScoreText(float score) {
            scoreText.color = textColors[playerIndex];
            int n = 0;
            while (n < score) {
                scoreText.text = $"{n}%";
                n += 1;
                yield return new WaitForSeconds(0.025f);
            }
        }
    }
}
