using System;
using System.Collections;
using System.Collections.Generic;
using Game.MinigameFramework.Scripts;
using Game.MinigameFramework.Scripts.Framework;
using Game.MinigameFramework.Scripts.Framework.Minigames;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MinigameManager : MonoBehaviour
{
    public static MinigameManager instance;

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }

        transform.parent = null;
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        SetupMinigameStatusDictionary();
        DetermineFewestPlayers();
        PopulateMinigameList();
        PawnBindingManager.onPauseButtonPressed.AddListener(OnPauseButton);
    }

    public SceneField minigameSelectScene;
    public SceneField resultsScene;
    public SceneField mainMenuScene;
    public PauseMenuButtons pauseMenuPrefab;
    
    [Header("Points")]
    public int pointsToWin = 20;
    public int pointsForFirstPlace = 4;
    public int pointsForSecondPlace = 3;
    public int pointsForThirdPlace = 2;
    public int pointsForFourthPlace = 1;

    [Header("Minigames")] public List<MinigamePack> allPacks = new();
    public List<MinigamePack> minigamePacks = new ();
    public List<MinigameInfo> minigames { get; private set; }
    public MinigameInfo debugMinigame;
    private Dictionary<MinigameInfo, bool> _minigameStatus = new();
    private bool _isMinigameEnded = false;

    // Sets expectedPlayers in PlayerManager
    // Called on start and when minigamePacks is updated
    public void DetermineFewestPlayers() {
        int min = PlayerManager.maxPlayers + 1; // could never occur

        // account for if debugMinigame assigned
        if (debugMinigame != null) {
            min = Mathf.Min(min, debugMinigame.minimumPlayers);
        } else {
            // otherwise, set min to the least restrictive value
            foreach(MinigamePack pack in minigamePacks) {
                foreach(MinigameInfo minigame in pack.minigames) {
                    if (!IsMinigameOn(minigame)) break;
                    min = Mathf.Min(min, minigame.minimumPlayers);
                }
            }
        }

        PlayerManager.expectedPlayers = min;
    }
    
    public void PopulateMinigameList() {
        minigames = new List<MinigameInfo>();

        if (debugMinigame != null) {
            minigames.Add(debugMinigame);
        }
        else {
            foreach(MinigamePack pack in minigamePacks) {
                foreach(MinigameInfo minigame in pack.minigames) {
                    if (!IsMinigameOn(minigame)) return;
                    if (minigame.minimumPlayers <= PlayerManager.GetNumPlayers()) {
                        minigames.Add(minigame);
                    }
                }
            }
        }
    }

    private void SetupMinigameStatusDictionary() {
        foreach (MinigamePack pack in allPacks) {
            foreach (MinigameInfo minigame in pack.minigames) {
                _minigameStatus.Add(minigame,true);
            }
        }
    }

    public void RemoveMinigameFromQueue(MinigameInfo minigameInfo) {
        minigames.Remove(minigameInfo);
        if(minigames.Count == 0) {
            PopulateMinigameList();
        }
    }
    
    public void GoToMinigameSelectScene() {
        SceneManager.LoadScene(minigameSelectScene.SceneName);
    }
    
    public void LoadMinigame(MinigameInfo minigame) {
        SceneManager.LoadScene(minigame.scene.SceneName);
        PlayerManager.SetMinigameActionMap();
        _isMinigameEnded = false;
    }
    
    public bool IsPackOn(MinigamePack pack) {
        return minigamePacks.Contains(pack);
    }
    
    public void TogglePack(MinigamePack pack) {
        if (!IsPackOn(pack)) minigamePacks.Add(pack);
        else minigamePacks.Remove(pack);
        DetermineFewestPlayers();
    }

    public void ToggleMinigame(MinigameInfo minigame) {
        _minigameStatus[minigame] = !_minigameStatus[minigame];
        DetermineFewestPlayers();
    }

    public bool IsMinigameOn(MinigameInfo minigame) {
        return _minigameStatus[minigame];
    } 
    
    
    public void EndMinigame(Ranking ranking) {
        if (_isMinigameEnded) {
            Debug.LogWarning("MinigameManager: EndMinigame has been called multiple times. Redundant request has been ignored.");
            return;
        }
        
        _isMinigameEnded = true;
        AwardPoints(ranking.ToList());
        PlayerManager.SetMenuActionMap();
        SceneManager.LoadScene(resultsScene.SceneName);
    }

    private void AwardPoints(List<int> ranking) {
        for(int playerIndex = 0; playerIndex < PlayerManager.players.Count; playerIndex++) {
            int playerRank = ranking[playerIndex];
            PlayerManager.players[playerIndex].points += GetPointsForRank(playerRank);
        }
    }
    private int GetPointsForRank(int rank) {
        switch(rank) {
            case 1:
                return pointsForFirstPlace;
            case 2:
                return pointsForSecondPlace;
            case 3:
                return pointsForThirdPlace;
            case 4:
                return pointsForFourthPlace;
            default:
                return 0;
        }
    }
    public void ResetPoints() {
        foreach(Player player in PlayerManager.players) {
            player.points = 0;
        }
    }
    
    /// <summary>
    /// Collection representing the placement of each player in a minigame.
    /// Index corresponds to player index with range [0-3]. Value corresponds to that player's rank with range [1-4] .
    /// For example, "ranking[3] = 1" sets Player 4 to first place.
    /// </summary>
    public class Ranking {
        private List<int> _playerRanks = new(4) { 0, 0, 0, 0 };

        public int this[int index] {
            get => _playerRanks[index];
            set => _playerRanks[index] = Math.Clamp(value, 1, 4);
        }

        public List<int> ToList() {
            return _playerRanks;
        }
        
        public int GetNextHighestRank() {
            int nextHighestRank = 1;
            
            if(_playerRanks.Contains(1)) {
                nextHighestRank = 2;
            } else if(_playerRanks.Contains(2)) {
                nextHighestRank = 3;
            } else if(_playerRanks.Contains(3)) {
                nextHighestRank = 4;
            }
            
            return nextHighestRank;
        }
        public int GetNextLowestRank() {
            int nextLowestRank = 4;
            if (_playerRanks.Contains(4)) {
                nextLowestRank = 3;
            } else if (_playerRanks.Contains(3)) {
                nextLowestRank = 2;
            } else if (_playerRanks.Contains(2)) {
                nextLowestRank = 1;
            }
            return nextLowestRank;
        }
        
        public void SetRank(int playerIndex, int rank) {
            this[playerIndex] = rank;
        }
        public void SetRanksFromList(int[] ranks) {
            for (int i = 0; i < ranks.Length; i++) {
                SetRank(i, ranks[i]);
            }
        }
        public void SetRanksFromPlayerIndexList(int[] playerIndexList) {
            if(playerIndexList.Length > 4) {
                throw new ArgumentException("Ranking::SetRank: parameter 'playerIndexList' length cannot be greater than 4");
            }
            for(int i = 0; i < 4; i++) {
                if (i >= playerIndexList.Length) break;
                if(playerIndexList[i] < 0) continue; // skip invalid player indices
                SetRank(playerIndexList[i], i + 1);
            }
        }
        public void SetAllPlayersToRank(int rank) {
            for(int i = 0; i<4; i++) {
                SetRank(i, rank);
            }
        }
        
        /// <summary>
        /// Ranks players from a list of scores ordered by player index.
        /// If players achieve the same score they will achieve the same rank.
        /// </summary>
        /// <param name="scores">Index in the list corresponds to player index</param>
        /// <returns></returns>
        public void DetermineRankingFromScores(List<int> scores) {
            _playerRanks = new(4) { 0, 0, 0, 0 };
            
            // Set up player index list
            List<int> playerIndexList = new();
            for(int i = 0; i < scores.Count; i++) {
                playerIndexList.Add(i);
            }
            // Sort player index list to be ordered by player score
            playerIndexList.Sort((a, b) => scores[b].CompareTo(scores[a]));
            // Determine rankings 
            SetRank(playerIndexList[0],1);
            print($"Player {playerIndexList[0]+1} ranked 1");
            for (int i = 1; i < playerIndexList.Count; i++) {
                int prevPlayerRank = this[playerIndexList[i - 1]];
                
                // Handle ties
                if (scores[playerIndexList[i]] == scores[playerIndexList[i - 1]]) {
                    SetRank(playerIndexList[i], prevPlayerRank);
                    print($"Player {playerIndexList[i]+1} ranked {this[playerIndexList[i]]}");
                    continue;
                }
                SetRank(playerIndexList[i], prevPlayerRank+1);
                print($"Player {playerIndexList[i]+1} ranked {this[playerIndexList[i]]}");
            }
        }
    }

    public void GoToMainMenuScene() {
        SceneManager.LoadScene(mainMenuScene.SceneName);
        PlayerManager.SetMenuActionMap();
    }
    private void OnPauseButton() {
        if(FindAnyObjectByType<PauseMenuButtons>() != null) {
            // Already paused
            return;
        }
        Time.timeScale = 0;
        Instantiate(pauseMenuPrefab);
        PlayerManager.SetMenuActionMap();
    }
    
    public List<Sprite> GetPackageSprites() {
        // if debugMinigame, just return its sprite
        // otherwise return the pack thumbnails for each selected pack
        List<Sprite> sprites = new List<Sprite>();
        if (debugMinigame != null) {
            sprites.Add(debugMinigame.thumbnail);
        } else {
            foreach(MinigamePack pack in minigamePacks) {
                sprites.Add(pack.icon);
            }
        }
        return sprites;
    }
    
}
