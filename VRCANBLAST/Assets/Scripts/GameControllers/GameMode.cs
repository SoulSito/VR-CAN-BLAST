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
    public int remainingCans;
    public int knockedCansInTable;

    public EndLevelResult(int remainingCans, int knockedCansInTable)
    {
        this.remainingCans = remainingCans;
        this.knockedCansInTable = knockedCansInTable;
    }
}

public class GameMode : MonoBehaviour
{
    public static GameMode Instance { get; private set; }

    [Header("Setup")]
    [SerializeField] List<LevelData> levels;

    [Header("References")]
    [SerializeField] PileOfCans pileOfCans;
    [SerializeField] UFO ufo;
    [SerializeField] ShootController pistol;

    [Header("UI")]
    [SerializeField] ScoreUI scoreUI;
    [SerializeField] SettingsUI settingsUI;


    public GameState isGameReady {get; private set;}

    int currentLevel = 0;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this.gameObject);

        isGameReady = GameState.Setup;
    }

    private void Start()
    {
        SetupGame();
    }

    private void SetupGame()
    {
        levels.Shuffle();

        isGameReady = GameState.ReadyToPlay;
    }

    internal void StartGame()
    {
        pistol.Setup(settingsUI.GetGunSettings());
        pileOfCans.PlaceLevel(levels[currentLevel]);
        ufo.Activate();

        currentLevel++;

        // TODO: Se levanta el toldo?
    }

    internal void PlaceNextPile()
    {
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
}
