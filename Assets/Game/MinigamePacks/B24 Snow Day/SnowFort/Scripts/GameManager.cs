using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SnowDay.Snowfort
{
    public class GameManager : MonoBehaviour
    {        
        public Scene[] maps;

        public PlayerController[] playerControllers;

        public TextMeshProUGUI timerText;

        [HideInInspector]
        public int leftTeam;
        [HideInInspector]
        public int rightTeam;

        GamePhase gamePhase = GamePhase.STANDBY;
        GamePhase nextGamePhase = GamePhase.DISTRIBUTION;

        float timer;

        int leftCores;
        int rightCores;

        bool firstBuildOccured;

        MusicManager musicManager;

        public bool MAP_OPEN_IN_EDITOR;

        public void StartGame()
        {
            foreach (Core core in FindObjectsOfType<Core>())
            {
                if (core.rightTeam)
                    rightCores++;
                else
                    leftCores++;
            }

            foreach (PlayerController player in playerControllers)
            {
                for (int i = 0; i < MapController.active.stock.Count; i++)
                {
                    player.cursor.AddObject(MapController.active.stock[i].obj, MapController.active.stock[i].count);
                }
            }
        }

        public void CoreDestroyed(bool rightCore)
        {
            if (rightCore)
                rightCores--;
            else 
                leftCores--;

            if (rightCores <= 0 || leftCores <= 0)
                SwitchPhase(true);
        }

        private void SwitchPhase(bool end = false)
        {
            Debug.Log("Switching Game Phase");

            if (end) nextGamePhase = GamePhase.STANDBY;

            gamePhase = nextGamePhase; 

            switch(nextGamePhase)
            {
                case GamePhase.STANDBY:
                    foreach (PlayerController player in playerControllers)
                    {
                        player.SetState(PlayerController.PlayerState.OBSERVER);
                    }

                    if (rightCores <= 0)
                    {
                        MinigameManager.Ranking r = new MinigameManager.Ranking();

                        r.SetRank(1, 2);
                        r.SetRank(3, 2);
                        r.SetRank(0, 1);
                        r.SetRank(2, 1);

                        MinigameManager.instance.EndMinigame(r);
                    }
                    else
                    {
                        MinigameManager.Ranking r = new MinigameManager.Ranking();

                        r.SetRank(1, 1);
                        r.SetRank(3, 1);
                        r.SetRank(0, 2);
                        r.SetRank(2, 2);

                        MinigameManager.instance.EndMinigame(r);
                    }
                    break;

                case GamePhase.DISTRIBUTION:
                    nextGamePhase = GamePhase.BUILD;
                    timer = MapController.active.distributionTime;

                    musicManager.SetIntenseMusic(true);

                    if (!firstBuildOccured) return;

                    foreach (PlayerController player in playerControllers)
                    {
                        player.SetState(PlayerController.PlayerState.OBSERVER);

                        for (int i = 0; i < MapController.active.restock.Count; i++)
                        {
                            player.cursor.AddObject(MapController.active.restock[i].obj, MapController.active.restock[i].count);
                        }

                        int r = Random.Range(0, MapController.active.bonus.Count);
                        player.cursor.AddObject(MapController.active.bonus[r].obj, MapController.active.bonus[r].count);
                    }
                    break;

                case GamePhase.BUILD:
                    nextGamePhase = GamePhase.COUNTDOWN;
                    timer = firstBuildOccured ? MapController.active.buildTime : MapController.active.firstBuildTime;
                    foreach (PlayerController player in playerControllers)
                    {
                        player.SetState(PlayerController.PlayerState.PLACEMENT);
                    }
                    firstBuildOccured = true;
                    break;

                case GamePhase.COUNTDOWN:
                    nextGamePhase = GamePhase.FIGHT;
                    timer = MapController.active.countdownTime;

                    musicManager.SetIntenseMusic(false);

                    foreach (PlayerController player in playerControllers)
                    {
                        player.SetState(PlayerController.PlayerState.OBSERVER);
                    }
                    break;

                case GamePhase.FIGHT:
                    nextGamePhase = GamePhase.DISTRIBUTION;
                    timer = MapController.active.fightTime;
                    foreach (PlayerController player in playerControllers)
                    {
                        player.SetState(PlayerController.PlayerState.BATTLE);
                    }
                    break;
            }
        }

        private void Update()
        {
            timer -= Time.deltaTime;
            if (timer <= 0) SwitchPhase();

            timerText.text = Mathf.FloorToInt(timer).ToString();

            switch (gamePhase)
            {
                case GamePhase.STANDBY:
                    break;

                case GamePhase.DISTRIBUTION:
                    break;

                case GamePhase.BUILD:
                    bool readyBuild = true;
                    foreach (PlayerController player in playerControllers)
                    {
                        if (player.playerState != PlayerController.PlayerState.OBSERVER && player.playerIndex != -1)
                            readyBuild = false;
                    }

                    if (readyBuild)
                        SwitchPhase();
                    break;

                case GamePhase.COUNTDOWN:
                    break;

                case GamePhase.FIGHT:
                    bool readyFight = true;
                    foreach (PlayerController player in playerControllers)
                    {
                        if (player.playerState != PlayerController.PlayerState.OBSERVER && player.playerIndex != -1)
                            readyFight = false;
                    }

                    if (readyFight)
                        SwitchPhase();
                    break;
            }
        }

        private void Start()
        {
            if (!MAP_OPEN_IN_EDITOR)
            {
                int r = Random.Range(0, maps.Length);
                SceneManager.LoadSceneAsync(r);
            }
        }

        private void Awake()
        {
            leftTeam = transform.GetChild(0).gameObject.layer;
            rightTeam = transform.GetChild(1).gameObject.layer;
            musicManager = FindObjectOfType<MusicManager>();
        }

        public enum GamePhase
        {
            STANDBY,
            DISTRIBUTION,
            BUILD,
            COUNTDOWN,
            FIGHT
        }
    }
}