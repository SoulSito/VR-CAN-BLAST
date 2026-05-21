using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {
    Setup,
    ReadyToPlay,
    Playing,
    Preparing,
    Finished
}

public class EndLevelResult
{
    public int untouchedCans;
    public int knockedCansInTable;

    public EndLevelResult(int untouchedCans = 0, int knockedCansInTable = 0)
    {
        this.untouchedCans = untouchedCans;
        this.knockedCansInTable = knockedCansInTable;
    }
}

public class GameMode : MonoBehaviour
{
    public static GameMode Instance { get; private set; }

    [Header("Setup")]
    [SerializeField] List<LevelData> LowQuantity;
    [SerializeField] List<LevelData> MediumQuantity;
    [SerializeField] List<LevelData> HighQuantity;
    [SerializeField] int timePerGame = 60;

    List<LevelData> levels;

    [Header("References")]
    [SerializeField] PileOfCans pileOfCans;
    [SerializeField] UFO ufo;
    [SerializeField] ShootController pistol;

    [Header("UI")]
    [SerializeField] ScoreUI scoreManager;
    [SerializeField] SettingsUI settingsUI;

    [Header("Scores")]
    [SerializeField] int KnockedCansInTableScore = 100;
    [SerializeField] int CansInFloorScore = 100;

    PlayerScore playerScore;

    public GameState gameState {get; private set;}

    int currentLevel = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        gameState = GameState.Setup;
    }

    private void Start()
    {
        switch (GameSettingsManager.Instance.cantidadDeLatas)
        {
            case GameSettingsManager.CantidadDeLatas.Baja:
                levels = LowQuantity;
                break;
            case GameSettingsManager.CantidadDeLatas.Media:
                levels = MediumQuantity;
                break;
            case GameSettingsManager.CantidadDeLatas.Alta:
                levels = HighQuantity;
                break;
            case GameSettingsManager.CantidadDeLatas.Aleatorio:
                levels = new List<LevelData>();
                levels.AddRange(LowQuantity);
                levels.AddRange(MediumQuantity);
                levels.AddRange(HighQuantity);
                break;
        }

        SetupGame();
    }

    private void SetupGame()
    {
        levels.Shuffle();

        gameState = GameState.ReadyToPlay;
    }

    internal void StartGame()
    {
        if (gameState != GameState.ReadyToPlay) return;

        pistol?.Setup(settingsUI.GetGunSettings());
        pileOfCans.PlaceLevel(levels[currentLevel]);
        ufo.Activate();

        currentLevel++;

        gameState = GameState.Playing;
        scoreManager?.StartTimer(timePerGame);
    }

    internal void PlaceNextPile()
    {
        if (gameState != GameState.Playing) return;

        gameState = GameState.Preparing;

        if(currentLevel >= levels.Count)
        {
            currentLevel = 0;
            levels.Shuffle();
        }

        EndLevelResult result = pileOfCans.EndLevel();
        scoreManager?.IncreaseScore(result.knockedCansInTable * KnockedCansInTableScore);
        pileOfCans.PlaceLevel(levels[currentLevel]);

        currentLevel++;
        gameState = GameState.Playing;
    }

    internal void OnCanWasPickedUp(GameObject can)
    {
        can.GetComponent<Can>().Disable();

        if(gameState != GameState.Playing)
        {
            pileOfCans.RemoveCanFromLists(can);

            Destroy(can);
        } else
        {
            pileOfCans.AddToCansPickedUp(can);
        }
    }

    internal void OnCanLeftTable(GameObject can)
    {
        if(gameState == GameState.Playing)
        {
            scoreManager?.IncreaseScore(CansInFloorScore);

            pileOfCans.AddCanToInactiveList(can);
            ufo.AddCanToPickUpList(can);
        }
    }

    internal void TimeEnded()
    {
        Debug.Log("Time Ended");

        gameState = GameState.Finished;

        playerScore = new PlayerScore(
            scoreManager.getScore(),
            GameSettingsManager.Instance.currentGunSettings.maxBullets,
            GameSettingsManager.Instance.cantidadDeLatas
            );

        SceneManager.LoadScene("ScoreScene");
    }

    internal PlayerScore GetPlayerScore()
    {
        if (playerScore != null) return playerScore;

        return new PlayerScore();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            PlaceNextPile();
        }
    }
}
