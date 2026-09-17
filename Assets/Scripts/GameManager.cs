using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    public static event Action<GameState> OnGameStateChanged;

    // RUN SPECIFIC VARS
    private int roundNumber = 0;
    private int enemiesElimatedThisRun = 0;
    private int maxChipsThisRun = 0;

    // PERSISTENT VALUES

    private int maxRoundsComplete;
    private int maxChipsAchiebed;
    private int maxEnemiesEliminated;
    private int runsPlayed;


    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        UpdateGameState(GameState.IN_START_MENU);
    }
    public void UpdateGameState(GameState _state)
    {
        State = _state;
        switch (State)
        {
            case GameState.TIPOFF:
                break;
            case GameState.BRIBE:
                /*if (roundNumber == 0)
                {
                    UpdateGameState(GameState.DEALING);
                }*/
                break;
            case GameState.DEALING:
                break;
            case GameState.PLAYER_TURN:
                break;
            case GameState.ENEMY1_TURN:
                break;
            case GameState.ENEMY2_TURN:
                break;
            case GameState.ENEMY3_TURN:
                break;
            case GameState.REVEAL:
                break;
            case GameState.END_ROUND:
                roundNumber++;
                break;
            default:
                break;
        }
        OnGameStateChanged?.Invoke(State);
    }

}
public enum GameState
{
    IN_START_MENU,
    TIPOFF,
    BRIBE,
    DEALING,
    PLAYER_TURN,
    ENEMY1_TURN,
    ENEMY2_TURN,
    ENEMY3_TURN,
    REVEAL,
    END_ROUND
}
