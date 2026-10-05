using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    public static event Action<GameState> OnGameStateChanged;

    public bool ignoreResets;

    // RUN SPECIFIC VARS
    public int RoundNumber = 0;
    public int OpponentsElimatedThisRun = 0;
    public int MaxChipsThisRun = 0;

    // PERSISTENT VALUES

    public int maxRoundsComplete;
    public int maxChipsAchieved;
    public int maxOpponentsEliminated;
    public int runsPlayed;

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
                ignoreResets = false; // Round over so resets need to happen
                break;
            case GameState.CHECK_IF_RUN_CONTINUE:
                if (BetManager.Instance.IsPlayerBankrupt())
                {
                    UpdateAllTimeStats();
                    // Trigger end of run state - this will display menu
                    UpdateGameState(GameState.END_OF_RUN_MENU);
                }
                else
                {
                    // Update run vars for end of round 
                    UpdateCurrentRunStats();
                    // Trigger end of round state - this will display menu
                    UpdateGameState(GameState.END_OF_ROUND_MENU);
                }
                
                break;
            case GameState.END_OF_ROUND_MENU:
                break;
            case GameState.END_OF_RUN_MENU:
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
    public void IncrementRunsPlayed()
    {
        runsPlayed++;
    }
    public void UpdateAllTimeStats()
    {
        IncrementRunsPlayed(); 

        maxOpponentsEliminated = Mathf.Max(maxOpponentsEliminated, OpponentsElimatedThisRun);
        maxChipsAchieved = Mathf.Max(maxChipsAchieved, MaxChipsThisRun);
        maxRoundsComplete = Mathf.Max(RoundNumber, maxRoundsComplete);
    }

    public void UpdateCurrentRunStats()
    {
        IncrementRoundNumber();
        UpdateMaxChipsThisRun();
        UpdateElimatedOpponentCount();

    }
    public void ResetRunStats()
    {
        RoundNumber = 0;
        OpponentsElimatedThisRun = 0;
        MaxChipsThisRun = 0;
    }
    public void UpdateElimatedOpponentCount()
    {
        OpponentsElimatedThisRun += BetManager.Instance.BankruptPlayers();
    }
    public void UpdateMaxChipsThisRun()
    {
        MaxChipsThisRun = Mathf.Max(BetManager.Instance.chips[(int)PlayerOrder.PLAYER], MaxChipsThisRun);
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
    CHECK_IF_RUN_CONTINUE,
    END_OF_ROUND_MENU,
    END_OF_RUN_MENU,
    NUM_OF_ROUNDS
}
