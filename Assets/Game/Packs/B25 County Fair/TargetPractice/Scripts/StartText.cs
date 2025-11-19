using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ShooterMinigame {
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class StartText : MonoBehaviour {

        private TextMeshProUGUI m_text;

        private float t = 0;

        private void Awake() {
            m_text = GetComponent<TextMeshProUGUI>();
            TargetPracticeManager.OnCountdownText += ShowText;
        }

        private void ShowText(string text) {
            t = 0;
            m_text.text = text;
        }

        private void Update() {
            m_text.transform.localScale = Vector3.one * (1 - (t * t));
            t += Time.deltaTime;
            t = Mathf.Clamp01(t);
        }
    }
}