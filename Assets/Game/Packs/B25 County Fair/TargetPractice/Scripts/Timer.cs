using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


namespace ShooterMinigame {
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class Timer : MonoBehaviour {

        private TextMeshProUGUI m_text;

        private void Awake() {
            m_text = GetComponent<TextMeshProUGUI>();
            TargetPracticeManager.OnTimerUpdated += UpdateTimer;
        }

        private void OnDestroy() {
            TargetPracticeManager.OnTimerUpdated -= UpdateTimer;
        }

        private void UpdateTimer(int value) {
            m_text.text = value.ToString();
        }
    }
}