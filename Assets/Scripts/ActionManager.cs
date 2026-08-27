using System.Collections.Generic;
using UnityEngine;


public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance;

    public bool[] hasFolded = new bool[4];
    public List<int> possibleActions = new List<int>();

    void Awake()
    {
        Instance = this;

        GameManager.OnGameStateChanged += GameManagerOnStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnStateChanged;
    }

    private void GameManagerOnStateChanged(GameState _state)
    {
        switch (_state)
        {
            case GameState.PLAYER_TURN:
                if (hasFolded[0])
                    GameManager.Instance.UpdateGameState(GameState.ENEMY1_TURN);
                break;
            case GameState.ENEMY1_TURN:
                if (hasFolded[1])
                    GameManager.Instance.UpdateGameState(GameState.ENEMY2_TURN);
                else
                    TakeTurn(1);
                break;
            case GameState.ENEMY2_TURN:
                if (hasFolded[2])
                    GameManager.Instance.UpdateGameState(GameState.ENEMY2_TURN);
                else
                    TakeTurn(2);
                break;
            case GameState.ENEMY3_TURN:
                if (hasFolded[3])
                {
                    // Check if pay should got back to player or reveal
                    HandleIfRoundCanEnd();
                }
                else
                    TakeTurn(3);
                break;
            case GameState.REVEAL:
                break;
            case GameState.END_ROUND:
                break;
            default:
                break;
        }
    }

    private void TakeTurn(int _playerNumber)
    {
        DeterminePossibleActionsForTurn(_playerNumber);
        int actionNumberIndex = Random.Range(0, possibleActions.Count);
        switch (possibleActions[actionNumberIndex])
        {
            case (int)Actions.CHECK:
                Check();
                break;
            case (int)Actions.CALL:
                Call(_playerNumber);
                break;
            case (int)Actions.RAISE:
                Raise(_playerNumber, RandomRaiseValue(_playerNumber));
                break;
            case (int)Actions.FOLD:
                Fold(_playerNumber);
                break;
            default:
                break;
        }
        Debug.Log($"Player {_playerNumber} performed action {possibleActions[actionNumberIndex]}");
    }
    private void DeterminePossibleActionsForTurn(int _playerNumber)
    {
        // Re-create lit of actions to chose from
        possibleActions.Clear();
        for (int i = 0; i < (int)Actions.NUM_OF_ACTIONS; i++)
        {
            possibleActions.Add(i);
        }
        // Determine if enemy can check this turn
        if (!CanCheck(_playerNumber))
        {
            possibleActions.Remove((int)Actions.CHECK);

            // If can't check must be something to call, determine if has enough chips to call
            if (BetManager.Instance.bets[_playerNumber] < BetManager.Instance.highestBet)
            {
                possibleActions.Remove((int)Actions.CALL);
            }

        }
        // If enemy can check it means there is nothing to call
        else
            possibleActions.Remove((int)Actions.CALL);

        // Determine if can raise
        if (BetManager.Instance.bets[_playerNumber] <= BetManager.Instance.highestBet)
            possibleActions.Remove((int)Actions.RAISE);
    }
    public void Check()
    {
        GameManager.Instance.UpdateGameState(GameManager.Instance.state + 1);
    }
    public bool CanCheck(int _playerNumber)
    {
        return BetManager.Instance.highestBet == BetManager.Instance.bets[_playerNumber];
    }
    public void Fold(int _playerNumber)
    {
        hasFolded[_playerNumber] = true;
        if(_playerNumber == (int)PlayerOrder.ENEMY3)
        {
            HandleIfRoundCanEnd();
        }
        else
            GameManager.Instance.UpdateGameState(GameManager.Instance.state + 1);
    }
    public void HandleIfRoundCanEnd()
    {
        // Check if everyone has matched any outstanding bets if not repeat rounds
        bool roundCanEnd = true;

        for (int i = 0; i < BetManager.Instance.bets.Length; i++)
        {
            if (hasFolded[i] == false && BetManager.Instance.bets[i] < BetManager.Instance.highestBet)
            {
                roundCanEnd = false;
            }
        }
        if (roundCanEnd)
            GameManager.Instance.UpdateGameState(GameManager.Instance.state + 1);
        else
            GameManager.Instance.UpdateGameState(GameState.PLAYER_TURN);
    }
    /// <summary>
    /// Checks for if call is valid shoud be performed before calling
    /// </summary>
    public void Call(int _playerNumber)
    {
        int chipsNeededToCall = BetManager.Instance.highestBet - BetManager.Instance.bets[_playerNumber];
        BetManager.Instance.chips[_playerNumber] -= chipsNeededToCall;
        BetManager.Instance.bets[_playerNumber] += chipsNeededToCall;
    }
    public void PlayerCall()
    {
        Call((int)PlayerOrder.PLAYER);
    }
    /// <summary>
    /// Checks for if call is valid shoud be performed before calling
    /// </summary>
    public void Raise(int _playerNumber, int _value)
    {
        BetManager.Instance.chips[_playerNumber] -= _value;
        BetManager.Instance.bets[_playerNumber] += _value;
        BetManager.Instance.highestBet = BetManager.Instance.bets[_playerNumber];
    }
    public int RandomRaiseValue(int _playerNumber)
    {
        int chipsToCall = BetManager.Instance.highestBet - BetManager.Instance.bets[_playerNumber];
        int chipsAvailableForRaise = BetManager.Instance.chips[_playerNumber] - chipsToCall;
        int incrementsAvailableForRaise = chipsAvailableForRaise / BetManager.Instance.minBet;
        int raiseIncrement = Random.Range(1, incrementsAvailableForRaise + 1);
        if (raiseIncrement == incrementsAvailableForRaise)
        {
            // TO DO: Trigger UI for All in
        }
        return raiseIncrement * BetManager.Instance.minBet;
    }
    public void PlayerRaise()
    {

    }
}  

public enum Actions
{
    CHECK, 
    CALL,
    RAISE,
    FOLD,
    NUM_OF_ACTIONS
}