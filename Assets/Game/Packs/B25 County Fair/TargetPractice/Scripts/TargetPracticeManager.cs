using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Linq;

namespace ShooterMinigame {
    public class TargetPracticeManager : MonoBehaviour {

        public static bool CanShoot { get; private set; } = false;

        public static System.Action<string> OnCountdownText = delegate { };
        public static System.Action<int> OnTimerUpdated = delegate { };

        [SerializeField]
        int m_gameDuration = 60;

        [SerializeField]
        ShooterMinigamePlayer[] m_players;

        [SerializeField]
        GameObject m_gameOverText;

        private void Start() {
            StartCoroutine(GameCoroutine());
        }

        private IEnumerator GameCoroutine() {
            yield return Countdown();
            yield return Timer();
            yield return GameOver();
        }

        private IEnumerator Countdown() {
            CanShoot = false;

            for (int i = 3; i > 0; i--) {
                OnCountdownText(i.ToString());
                yield return new WaitForSecondsRealtime(1);
            }

            CanShoot = true;
            OnCountdownText("Go!");
        }

        private IEnumerator Timer() {
            for (int i = m_gameDuration; i > 0; i--) {
                OnTimerUpdated(i);

                if (i % 5 == 0) {
                    Target.ActivateTargets((m_gameDuration - i) / 15 + 4);
                }

                yield return new WaitForSecondsRealtime(1);
            }

            OnTimerUpdated(0);
        }

        private IEnumerator GameOver() {
            m_gameOverText.SetActive(true);
            CanShoot = false;
            yield return new WaitForSecondsRealtime(2);

            m_players = m_players.OrderBy(player => -player.score).ToArray();

            int rank = 1;
            var ranking = new MinigameManager.Ranking();

            for (int i = 0; i < m_players.Length; i++) {
                var player = m_players[i];
                
                if (player.playerIndex == -1)
                    continue;

                ranking[player.playerIndex] = rank;
                if (i < 3 && m_players[i+1].score != player.score)
                    rank++;
            }

            MinigameManager.instance.EndMinigame(ranking);
        }
    }
}