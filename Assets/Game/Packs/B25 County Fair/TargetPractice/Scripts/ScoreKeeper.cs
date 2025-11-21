using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace ShooterMinigame {
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class ScoreKeeper : MonoBehaviour {

        [SerializeField]
        ShooterMinigamePlayer m_player;

        private TextMeshProUGUI m_text;

        private void Awake() {
            m_text = GetComponent<TextMeshProUGUI>();
        }

        void Start() {
            m_player.onScoreUpdated += UpdateScore;
        }

        private void OnDestroy() {
            m_player.onScoreUpdated -= UpdateScore;
        }

        private void UpdateScore(int score) {
            m_text.text = score.ToString();
        }
    }
}