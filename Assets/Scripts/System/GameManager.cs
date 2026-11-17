using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    public static event Action<GameState> OnGameStateChanged;

    public bool ignoreResets;

    // Values for last winners
    public List<PlayerOrder> winners = new List<PlayerOrder>();
    public HandType winningHandType;

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

    public static string GetPlayerName(PlayerOrder _player)
    {
        switch (_player)
        {
            case PlayerOrder.PLAYER:
                return "You";
            case PlayerOrder.OPPONENT1:
                return "Opponent 1";
            case PlayerOrder.OPPONENT2:
                return "Opponent 2";
            case PlayerOrder.OPPONENT3:
                return "Opponent 3";
            default:
                break;
        }
        return "";
    }

    public static string GetHandName(HandType _handType)
    {
        switch(_handType) 
        {
            case HandType.HIGH_CARD:
                return ("High Card");
            case HandType.PAIR:
                return ("Pair");
            case HandType.TWO_PAIR:
                return ("Two Pair");
            case HandType.THREE_OF_A_KIND:
                return ("Three of a Kind");
            case HandType.STRAIGHT:
                return ("Straight");
            case HandType.FLUSH:
                return ("Flush");
            case HandType.FULL_HOUSE:
                return ("Full House");
            case HandType.FOUR_OF_A_KIND:
                return ("Four of a Kind");
            case HandType.STRAIGHT_FLUSH:
                return ("Straight Flush");
        }
        return "";
    }
}
public enum GameState
{
    IN_START_MENU,
    TIPOFF,
    BRIBE,
    SHUFFLING,
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
