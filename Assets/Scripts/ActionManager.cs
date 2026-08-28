using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance;

    public bool[] hasFolded = new bool[(int)PlayerOrder.NUM_OF_PLAYERS];
    public int foldedPlayers;
    public bool[] isAllIn = new bool[(int)PlayerOrder.NUM_OF_PLAYERS];

    public Actions[] lastActions = new Actions[(int)PlayerOrder.NUM_OF_PLAYERS];
    
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
        Debug.Log($"Game state changed to {GameManager.Instance.state} ");
        switch (GameManager.Instance.state)
        {
            case GameState.DEALING:
                ResetPlayerStates();
                break;
            case GameState.PLAYER_TURN:
                if (TurnNeedsSkipping((int)PlayerOrder.PLAYER))
                    MoveToNextStageWithoutDelay((int)PlayerOrder.PLAYER);
                break;

            case GameState.ENEMY1_TURN:
                if (TurnNeedsSkipping((int)PlayerOrder.ENEMY1))
                    MoveToNextStageWithoutDelay((int)PlayerOrder.ENEMY1);
                else
                    TakeTurn((int)PlayerOrder.ENEMY1);
                break;

            case GameState.ENEMY2_TURN:
                if (TurnNeedsSkipping((int)PlayerOrder.ENEMY2))
                    MoveToNextStageWithoutDelay((int)PlayerOrder.ENEMY2);
                else
                    TakeTurn((int)PlayerOrder.ENEMY2);
                break;

            case GameState.ENEMY3_TURN:
                if (TurnNeedsSkipping((int)PlayerOrder.ENEMY3))
                {
                    MoveToNextStageWithoutDelay((int)PlayerOrder.ENEMY3);
                }
                else
                    TakeTurn((int)PlayerOrder.ENEMY3);
                break;

            default:
                break;
        }
    }

    public void ResetPlayerStates()
    {
        hasFolded = new bool[(int)PlayerOrder.NUM_OF_PLAYERS];
        isAllIn = new bool[(int)PlayerOrder.NUM_OF_PLAYERS];
        lastActions = new Actions[(int)PlayerOrder.NUM_OF_PLAYERS];
        foldedPlayers = 0;
    }
    public void TakeTurn(int _playerNumber)
    {
        DeterminePossibleActionsForTurn(_playerNumber);

        int actionNumberIndex = Random.Range(0, possibleActions.Count);
        switch (possibleActions[actionNumberIndex])
        {
            case (int)Actions.CHECK:
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

        lastActions[_playerNumber] = (Actions)possibleActions[actionNumberIndex];
        Debug.Log($"Player {(PlayerOrder)_playerNumber} performed action {(Actions)possibleActions[actionNumberIndex]}");

        // Check if only one player remaining ask skip to end
        if (foldedPlayers == (int)PlayerOrder.NUM_OF_PLAYERS - 1)
            MoveToNextStageWithoutDelay(_playerNumber);
        else
            StartCoroutine(MoveToNextStageWithDelay(_playerNumber));
    }
    public bool TurnNeedsSkipping(int _playerNumber)
    {
        return foldedPlayers == (int)PlayerOrder.NUM_OF_PLAYERS-1 || // All other players have folded so skip to reveal
            hasFolded[_playerNumber] ||
            isAllIn[_playerNumber] ||
            (BetManager.Instance.highestBet > 0 && BetManager.Instance.bets[_playerNumber] == BetManager.Instance.highestBet); // Check if turn should be skipped as player is being revisited as others had to call their last bet
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
        }
        // If enemy can check it means there is nothing to call and shouldn't fold
        else
        {
            possibleActions.Remove((int)Actions.CALL);
            possibleActions.Remove((int)Actions.FOLD);
        }

        // Determine if can't physically raise OR shouldn't be allowed to raise (previously players calling their last bet)
        if (!CanRaise(_playerNumber))
            possibleActions.Remove((int)Actions.RAISE);

        // REMOVE
        /*foreach (int action in possibleActions)
        {
            Debug.Log($"Player {(PlayerOrder)_playerNumber} can perform {(Actions)action}");
        }*/
    }
    public bool CanCheck(int _playerNumber)
    {
        return BetManager.Instance.highestBet == BetManager.Instance.bets[_playerNumber];
    }
    public bool CanRaise(int _playerNumber)
    {
        // 1st raise should be allowed always - as long as player has chips
        if (BetManager.Instance.highestBet == 0 && BetManager.Instance.chips[_playerNumber] >= BetManager.Instance.minBet)
            return true;

        // There are existing bets, determine if can physically raise 
        return BetManager.Instance.chips[_playerNumber] >= (BetManager.Instance.highestBet - BetManager.Instance.bets[_playerNumber]) + BetManager.Instance.minBet;
    }
    public bool CanCallOrFold(int _playerNumber)
    {
        return BetManager.Instance.bets[_playerNumber] < BetManager.Instance.highestBet;
    }
    public void Fold(int _playerNumber)
    {
        hasFolded[_playerNumber] = true;
        foldedPlayers++;
    }
    public void PlayerFold()
    {
        Fold((int)PlayerOrder.PLAYER);
        lastActions[(int)PlayerOrder.PLAYER] = Actions.FOLD;

        StartCoroutine(MoveToNextStageWithDelay((int)PlayerOrder.PLAYER));
    }
    public IEnumerator MoveToNextStageWithDelay(int _playerNumber)
    {
        yield return new WaitForSecondsRealtime(1.0f);

        if (_playerNumber == (int)PlayerOrder.ENEMY3)
        {
            HandleIfRoundCanEnd();
        }
        else
            GameManager.Instance.UpdateGameState((GameState)(GameManager.Instance.state + 1));
    }
    public void MoveToNextStageWithoutDelay(int _playerNumber)
    {
        if (_playerNumber == (int)PlayerOrder.ENEMY3)
        {
            HandleIfRoundCanEnd();
        }
        else
            GameManager.Instance.UpdateGameState((GameState)(GameManager.Instance.state + 1));
    }
    public void PlayerCheck()
    {
        lastActions[(int)PlayerOrder.PLAYER] = Actions.CHECK;

        StartCoroutine(MoveToNextStageWithDelay((int)(PlayerOrder.PLAYER)));
    }
    public void HandleIfRoundCanEnd()
    {
        // Check if everyone has matched any outstanding bets if not repeat rounds
        bool roundCanEnd = true;

        for (int i = 0; i < BetManager.Instance.bets.Length; i++)
        {
            if (hasFolded[i] == false && isAllIn[i] == false &&  BetManager.Instance.bets[i] < BetManager.Instance.highestBet)
            {
                roundCanEnd = false;
            }
        }
        if (roundCanEnd)
            GameManager.Instance.UpdateGameState((GameState)(GameManager.Instance.state + 1));
        else
            GameManager.Instance.UpdateGameState(GameState.PLAYER_TURN);
    }
    /// <summary>
    /// Checks for if call is valid shoud be performed before calling
    /// </summary>
    public void Call(int _playerNumber)
    {
        int chipsNeededToCall = BetManager.Instance.highestBet - BetManager.Instance.bets[_playerNumber];

        // Player does not have enough chips to call so must go all in
        if (chipsNeededToCall >= BetManager.Instance.chips[_playerNumber])
        {
            BetManager.Instance.bets[_playerNumber] += BetManager.Instance.chips[_playerNumber];
            BetManager.Instance.pot += BetManager.Instance.chips[_playerNumber];

            BetManager.Instance.chips[_playerNumber] = 0;
            isAllIn[_playerNumber] = true;
        }
        else
        {
            BetManager.Instance.chips[_playerNumber] -= chipsNeededToCall;
            BetManager.Instance.bets[_playerNumber] += chipsNeededToCall;
            BetManager.Instance.pot += chipsNeededToCall;
        }
    }
    public void PlayerCall()
    {
        Call((int)PlayerOrder.PLAYER);
        lastActions[(int)PlayerOrder.PLAYER] = Actions.CALL;

        StartCoroutine(MoveToNextStageWithDelay((int)PlayerOrder.PLAYER));
    }
    /// <summary>
    /// Checks for if call is valid shoud be performed before calling
    /// </summary>
    public void Raise(int _playerNumber, int _value)
    {
        Call(_playerNumber);

        BetManager.Instance.chips[_playerNumber] -= _value;
        BetManager.Instance.bets[_playerNumber] += _value;
        BetManager.Instance.pot += _value;
        BetManager.Instance.highestBet = BetManager.Instance.bets[_playerNumber];
    }
    public int RandomRaiseValue(int _playerNumber)
    {
        // Calculate chips available to raise with after any outstanding call player needs to make
        int chipsNeededToCall = BetManager.Instance.highestBet - BetManager.Instance.bets[_playerNumber];
        int availableChips = BetManager.Instance.chips[_playerNumber] - chipsNeededToCall;

        int incrementsAvailableForRaise = availableChips / BetManager.Instance.minBet;
        int raiseIncrement = Random.Range(1, incrementsAvailableForRaise + 1);
        if (raiseIncrement == incrementsAvailableForRaise)
        {
            isAllIn[_playerNumber] = true;
            // TO DO: Trigger UI for All in
        }
        Debug.Log($"Player {_playerNumber} raised by {raiseIncrement * BetManager.Instance.minBet}");
        return raiseIncrement * BetManager.Instance.minBet;
    }
    public void PlayerRaise(int _value)
    {
        Raise((int)PlayerOrder.PLAYER, _value);


        lastActions[(int)PlayerOrder.PLAYER] = Actions.RAISE;
        StartCoroutine(MoveToNextStageWithDelay((int)PlayerOrder.PLAYER));
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