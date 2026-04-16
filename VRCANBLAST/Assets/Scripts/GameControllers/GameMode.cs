using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public enum GameState {
    Setup,
    ReadyToPlay,
    Playing,
    Finished
}

public class EndLevelResult
{
    public int untouchedCans;
    public int knockedCansInTable;

    public EndLevelResult(int untouchedCans, int knockedCansInTable)
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
    [SerializeField] ScoreUI scoreUI;
    [SerializeField] SettingsUI settingsUI;


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
        pistol?.Setup(settingsUI.GetGunSettings());
        pileOfCans.PlaceLevel(levels[currentLevel]);
        ufo.Activate();

        currentLevel++;

        gameState = GameState.Playing;
        scoreUI.StartTimer(timePerGame);

        // TODO: Se levanta el toldo?
    }

    internal void PlaceNextPile()
    {
        if(currentLevel > levels.Count - 1)
        {
            currentLevel = 0;
            levels.Shuffle();
        }

        EndLevelResult result = pileOfCans.EndLevel();
        pileOfCans.PlaceLevel(levels[currentLevel]);

        currentLevel++;
    }

    internal void OnCanWasPickedUp(GameObject can)
    {
        Can canScript = can.GetComponent<Can>();

        canScript.Disable();
        pileOfCans.AddCanToInactiveList(can);
    }

    internal void OnCanTouchedFloor(GameObject can)
    {
        scoreUI.IncreaseScore(100);

        ufo.AddCanToPickUpList(can);
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
            OnCanTouchedFloor(null);
        }

        if (Input.GetKey(KeyCode.F3))
        {
            PlaceNextPile();
        }
    }
}
