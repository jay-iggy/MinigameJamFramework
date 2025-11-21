using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using XiaoHuanXiong.Common;
using XiaoHuanXiong.Player;
using XiaoHuanXiong.UI;

namespace XiaoHuanXiong.Game
{
    public enum GameMode
    {
        BeforeStart,
        Playing,
        Ending,
    }

    public class GameManager : Singleton<GameManager>
    {
        //Game mode
        public GameMode currentGameMode { get; private set; }

        public event Action<GameMode> OnGameModeChanged;

        private GameMode _lastGameMode;
        //public  RaccoonPawn[] _players;
        public List<RaccoonPawn> _players = new List<RaccoonPawn>();

        // Time Variables
        [Header("Time")]
        public int duration = 120;
        public CountDownTimer countDownTimer;
        [HideInInspector] public int _timer = 0;
        
        // Scoring Variables
        private MinigameManager.Ranking _ranking = new MinigameManager.Ranking();
        private int _deaths = 0;
        private void Start()
        {
            //_players = FindObjectsOfType<RaccoonPawn>();

            _ranking.SetAllPlayersToRank(1);
            currentGameMode = GameMode.Playing;
            StartCoroutine(GameTimer());
        }
        IEnumerator GameTimer()
        {
           
            // Timer
            while (_timer < duration)
            {
                _timer += 1;
                countDownTimer.UpdateCountDownTimer(duration-_timer);
                yield return new WaitForSeconds(1f);
            }
            StartCoroutine(EndMinigame());
        }
        public void SetGameMode(GameMode mode)
        {
            if (currentGameMode != mode)
            {
                currentGameMode = mode;
                OnGameModeChanged?.Invoke(mode);
            }
        }
        private List<int> GetAllPlayerScores()
        {
            List<int> scores = new List<int>();

            foreach (var player in PlayerManager.players)
            {

                scores.Add(_players[player.playerIndex].Score);   
            }

            return scores;
        }

        public void UpdateRanking() {
            
            List<int> scores = GetAllPlayerScores();
            _ranking.DetermineRankingFromScores(scores);
        }
        IEnumerator EndMinigame()
        {
            
            UpdateRanking();
            MinigameManager.instance.EndMinigame(_ranking);
            yield return null;
        }
    }

}
