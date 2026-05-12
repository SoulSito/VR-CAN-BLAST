using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] List<LevelData> levels;
    [SerializeField] int timePerGame = 60;

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

    public GameState gameState {get; private set;}

    int currentLevel = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);

        gameState = GameState.Setup;
    }

    private void Start()
    {
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
        gameState = GameState.Finished;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            StartGame();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            OnCanLeftTable(null);
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            PlaceNextPile();
        }
    }
}
