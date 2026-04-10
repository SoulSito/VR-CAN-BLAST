using UnityEngine;

public enum GameState {
    Setup,
    ReadyToPlay,
    Playing,
    Finished
}

public class GameMode : MonoBehaviour
{
    public static GameMode Instance { get; private set; }

    [SerializeField] PileOfCans pileOfCans;

    public GameState isGameReady {get; private set;}

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
        if (pileOfCans != null) pileOfCans.Setup();

        isGameReady = GameState.ReadyToPlay;
    }

    public void StartGame()
    {

    }

    public void RestartGame()
    {
        pileOfCans.Reset();
    }

    public void OnPlayerRanOutOfBullets(GameObject player)
    {

    }
}
