using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    public static event Action<GameState> OnGameStateChanged;

    // RUN SPECIFIC VARS
    public int RoundNumber = 0;
    public int EnemiesElimatedThisRun = 0;
    public int MaxChipsThisRun = 0;

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
            case GameState.OPPONENT1_TURN:
                break;
            case GameState.OPPONENT2_TURN:
                break;
            case GameState.OPPONENT3_TURN:
                break;
            case GameState.REVEAL:
                break;
            case GameState.END_ROUND:
                break;
            case GameState.ON_MENU_BETWEEN_ROUNDS:
                if (BetManager.Instance.IsPlayerBankrupt())
                {
                    // TO DO: Display results menu, save data, allow option to start new run
                }
                else
                {
                    // TO DO: Display menu to go on next round, show current stats 
                }
                break;
            default:
                break;
        }
        OnGameStateChanged?.Invoke(State);
    }
    public void IncrementRoundNumber()
    {
        RoundNumber++;
    }
    public void UpdateElimatedEnemyCount()
    {
        EnemiesElimatedThisRun += BetManager.Instance.BankruptPlayers();
    }

}
public enum GameState
{
    IN_START_MENU,
    TIPOFF,
    BRIBE,
    DEALING,
    PLAYER_TURN,
    OPPONENT1_TURN,
    OPPONENT2_TURN,
    OPPONENT3_TURN,
    REVEAL,
    END_ROUND, 
    ON_MENU_BETWEEN_ROUNDS,
    NUM_OF_ROUNDS
}
