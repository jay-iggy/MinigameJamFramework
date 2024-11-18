using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using Game.MinigameFramework.Scripts.Framework.PlayerInfo;

public class CrowdGameManager : MonoBehaviour
{
    public static CrowdGameManager inst;

    public Vector2 boundaries = new Vector2(17f, 8.25f);
    public Vector2 center = new Vector2(0, -7.5f/2f);
    [SerializeField] private GameObject pedestrianPrefab;
    [SerializeField] private int numStarting = 65, numPerRound = 10;
    [SerializeField] private int numRounds = 3;
    [SerializeField] private List<CrowdPlayerPawn> players = new List<CrowdPlayerPawn>();
    private int[] playerScores;
    [SerializeField] private List<TextMeshProUGUI> scoreDisplays = new();
    [SerializeField] private GameObject validationPrefab;
    
    [SerializeField] private TextMeshProUGUI roundDisplay;
    [SerializeField] private TextMeshProUGUI phaseTimerText;
    private int round = 1;

    public UnityEvent sceneChanged;

    public enum gamePhase {
        Intro,
        Search,
        Select,
        Score,
        Rank
    }

    public gamePhase currentGamePhase = gamePhase.Intro;
    private float phaseTimer;
    public float introDuration = 4;
    [SerializeField] private float searchDuration = 15, selectDuration = 10, scoreDuration = 5, rankDuration = 5;
    private List<PedestrianBehavior> pedestrians = new List<PedestrianBehavior>();

    private void Awake() {
        inst = this;
    }

    void Start() {
        SpawnPedestrians(numStarting);

        // Remove disconnected players
        if (PlayerManager.players.Count != 0)
            for (int i = 3 ; i > PlayerManager.players.Count - 1 ; i--) {
                Destroy(players[i].gameObject);
                players.RemoveAt(i);

            }

        playerScores = new int[players.Count];
 
        currentGamePhase = gamePhase.Intro;
        phaseTimer = introDuration;
    }

    private void SpawnPedestrians(int num) {
        for (int i = 0; i < num; i++) {
            Vector2 ranPos = new Vector2(
                Random.Range(-boundaries.x, boundaries.x),
                Random.Range(-boundaries.y, boundaries.y)
            ) + center;
            ranPos += Vector2.right * Mathf.Sign(ranPos.x) * boundaries.x * 1.5f;
            GameObject pedestrian = Instantiate(pedestrianPrefab, ranPos, Quaternion.identity);
            pedestrians.Add(pedestrian.GetComponent<PedestrianBehavior>());
        }
    }

    private void Update() {
        if (phaseTimer > 0) {
            if (currentGamePhase == gamePhase.Score)
                phaseTimerText.text = "0";
            else
                phaseTimerText.text = "" + Mathf.Ceil(phaseTimer);
            phaseTimer -= Time.deltaTime;
            if (currentGamePhase == gamePhase.Select)
            {
                int selectCount = 0;
                foreach (CrowdPlayerPawn player in players)
                {
                    if (player.HighlightedAny())
                    {
                        selectCount++;
                    }
                }
                if (selectCount == players.Count)
                {
                    phaseTimer = Mathf.Clamp(phaseTimer, 0, 1.25f);
                }
            }
        } else {
            phaseTimerText.text = "0";
            if (currentGamePhase == gamePhase.Intro) {
                currentGamePhase = gamePhase.Search;
                phaseTimer = searchDuration;

                foreach (PedestrianBehavior pedestrian in pedestrians) {
                    pedestrian.ChooseMovementType();
                }
            } else if (currentGamePhase == gamePhase.Search) {
                currentGamePhase = gamePhase.Select;
                phaseTimer = selectDuration;
            } else if (currentGamePhase == gamePhase.Select) {
                currentGamePhase = gamePhase.Score;
                phaseTimer = scoreDuration;

                ScoreSelections();
            } else if (currentGamePhase == gamePhase.Score) {
                round++;
                if (round > numRounds) {
                    currentGamePhase = gamePhase.Rank;
                    phaseTimer = rankDuration;
                    SubmitScores();
                } else {
                    currentGamePhase = gamePhase.Intro;
                    phaseTimer = introDuration;
                    roundDisplay.text = "Round " + round;
                    SpawnPedestrians(numPerRound);
                }
            }
            sceneChanged.Invoke();
        }
    }

    public PedestrianBehavior ChooseRandomPlayer(int requestingPlayerIndex) {
        PedestrianBehavior foundPedestrian = null;
        while (foundPedestrian == null || foundPedestrian.IsPlayer()) {
            foundPedestrian = pedestrians[Random.Range(0, pedestrians.Count)];
        }
        foundPedestrian.MakePlayer(requestingPlayerIndex);
        if (foundPedestrian == null) Debug.Log("ERROR: no non-player found");
        return foundPedestrian;
    }

    List<CrowdPlayerPawn> stolenPlayers = new List<CrowdPlayerPawn>();
    List<CrowdPlayerPawn> correctPlayers = new List<CrowdPlayerPawn>();
    List<CrowdPlayerPawn> wrongPlayers = new List<CrowdPlayerPawn>();
    List<CrowdPlayerPawn> inactivePlayers = new List<CrowdPlayerPawn>();
    private void ScoreSelections() {
        stolenPlayers = new List<CrowdPlayerPawn>();
        correctPlayers = new List<CrowdPlayerPawn>();
        wrongPlayers = new List<CrowdPlayerPawn>();
        inactivePlayers = new List<CrowdPlayerPawn>();

        foreach (CrowdPlayerPawn player in players) {
            if (player.HighlightedAny()) {
                if (player.HighlightedStolen()) {
                    stolenPlayers.Add(player);
                } else if (player.HighlightedCorrectPedestrian()) {
                    correctPlayers.Add(player);
                } else {
                    wrongPlayers.Add(player);
                }
            } else {
                inactivePlayers.Add(player);
            }
        }

        if (stolenPlayers.Count > 1) stolenPlayers.Sort();
        if (correctPlayers.Count > 1) correctPlayers.Sort();
        if (wrongPlayers.Count > 1) wrongPlayers.Sort();
        if (inactivePlayers.Count > 1) Shuffle(inactivePlayers);

        List<CrowdPlayerPawn> orderedPlayers = new List<CrowdPlayerPawn>();
        orderedPlayers.AddRange(stolenPlayers);
        orderedPlayers.AddRange(correctPlayers);
        orderedPlayers.AddRange(wrongPlayers);
        orderedPlayers.AddRange(inactivePlayers);

        for (int i = 0; i < playerScores.Length; i++) {
            int bonus = i < stolenPlayers.Count ? 2 : 0;
            playerScores[orderedPlayers[i].playerPawnIndex] += playerScores.Length - i + bonus;
        }

        StartCoroutine("DisplayAccuracy");
    }

    IEnumerator DisplayAccuracy() {
        foreach (CrowdPlayerPawn player in stolenPlayers) {
            ValidationBehavior vb = Instantiate(
                validationPrefab,
                player.GetHighlightedPosition(),
                Quaternion.identity
            ).GetComponent<ValidationBehavior>();
            vb.SetVisualsStolen(player.GetColor());
            AudioManager.inst.PlayCorrect();

            yield return new WaitForSeconds(scoreDuration / players.Count);
        }

        foreach (CrowdPlayerPawn player in correctPlayers) {
            ValidationBehavior vb = Instantiate(
                validationPrefab,
                player.GetHighlightedPosition(),
                Quaternion.identity
            ).GetComponent<ValidationBehavior>();
            vb.SetVisuals(true, player.GetColor());
            AudioManager.inst.PlayCorrect();

            yield return new WaitForSeconds(scoreDuration / players.Count);
        }

        List<CrowdPlayerPawn> incorrectPlayers = wrongPlayers;
        incorrectPlayers.AddRange(inactivePlayers);
        foreach (CrowdPlayerPawn player in wrongPlayers) {
            ValidationBehavior vb = Instantiate(
                validationPrefab,
                player.GetHighlightedPosition(),
                Quaternion.identity
            ).GetComponent<ValidationBehavior>();
            vb.SetVisuals(false, player.GetColor());
            AudioManager.inst.PlayIncorrect();

            yield return new WaitForSeconds(scoreDuration / players.Count);
        }

        for (int i = 0; i < playerScores.Length; i++) {
            scoreDisplays[i].text = "" + playerScores[i];
        }
    }

    void Shuffle(List<CrowdPlayerPawn> ts) {
		var count = ts.Count;
		var last = count - 1;
		for (var i = 0; i < last; ++i) {
			var r = UnityEngine.Random.Range(i, count);
			var tmp = ts[i];
			ts[i] = ts[r];
			ts[r] = tmp;
		}
	}

    MinigameManager.Ranking ranking = new();

    void SubmitScores() {
        List<int> rearrangableScores = new List<int>();
        foreach (int score in playerScores) {
            rearrangableScores.Add(score);
        }

        for (int j = 0; j < playerScores.Length; j++) {
            int scoreMax = -1;
            int scoreMaxIndex = -1;
            for (int i = 0; i < playerScores.Length; i++) {
                int score = rearrangableScores[i];
                if (score > scoreMax) {
                    scoreMax = score;
                    scoreMaxIndex = i;
                }
            }
            ranking.SetRank(scoreMaxIndex, j + 1);
            rearrangableScores[scoreMaxIndex] = -1;
        }

        StartCoroutine("DisplayRankings");
    }

    IEnumerator DisplayRankings() {
        roundDisplay.text = "Finished";

        CrowdPlayerPawn[] playersInRankOrder = new CrowdPlayerPawn[players.Count];
        foreach (CrowdPlayerPawn player in players) {
            playersInRankOrder[players.Count - ranking.playerRanks[player.playerPawnIndex]] = player;
        }

        foreach (CrowdPlayerPawn player in playersInRankOrder) {
            ValidationBehavior vb = Instantiate(
                validationPrefab,
                player.GetPointerPosition(),
                Quaternion.identity
            ).GetComponent<ValidationBehavior>();
            vb.UseRank(ranking.playerRanks[player.playerPawnIndex], player.GetColor());
            AudioManager.inst.PlayHighlight();

            yield return new WaitForSeconds(rankDuration / players.Count);
        }

        MinigameManager.instance.EndMinigame(ranking);
    }

    public float GetRemainingPhaseTime() {
        return phaseTimer;
    }
}
