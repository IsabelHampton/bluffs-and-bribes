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

    public void UpdateAllIn(int _playerNumber)
    {
        isAllIn[_playerNumber] = true;
        lastActions[_playerNumber] = Actions.ALL_IN;
    }

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
        Debug.Log($"Game state changed to {GameManager.Instance.State} ");
        switch (GameManager.Instance.State)
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
                Check(_playerNumber);
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

        //lastActions[_playerNumber] = (Actions)possibleActions[actionNumberIndex];
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
        // Don't include all in as a possible actions
        for (int i = 0; i < (int)Actions.NUM_OF_ACTIONS - 1; i++)
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
    }

    public void FindHandSoFar(int _playerNumber)
    {
        // Get all visible cards in middle
        List<Card> visibleCards = new List<Card>();
        foreach (Card card in HandManager.Instance.centreCards)
        {
            if (card.faceDown)
                break; // stop loop no more visible cards
            else
                visibleCards.Add(card);
        }
        // Add cards in player's hand
        foreach (Card card in HandManager.Instance.allHands[_playerNumber])
            visibleCards.Add(card);
        // Detemrine hand rank
        HandManager.Instance.DetermineCurrentHandRank(visibleCards.ToArray(), _playerNumber);

        // Output player's current hand
        Debug.Log($"Player currently has {(HandType)HandManager.Instance.playerHands[_playerNumber].handType} with rank {HandManager.Instance.playerHands[_playerNumber].handType}");
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
        lastActions[_playerNumber] = Actions.FOLD;
    }
    public void PlayerFold()
    {
        Fold((int)PlayerOrder.PLAYER);

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
            GameManager.Instance.UpdateGameState((GameState)(GameManager.Instance.State + 1));
    }
    public void MoveToNextStageWithoutDelay(int _playerNumber)
    {
        if (_playerNumber == (int)PlayerOrder.ENEMY3)
        {
            HandleIfRoundCanEnd();
        }
        else
            GameManager.Instance.UpdateGameState((GameState)(GameManager.Instance.State + 1));
    }
    public void Check(int _playerNumber)
    {
        lastActions[_playerNumber] = Actions.CHECK;
    }
    public void PlayerCheck()
    {
        Check((int)PlayerOrder.PLAYER);

        StartCoroutine(MoveToNextStageWithDelay((int)PlayerOrder.PLAYER));
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
            GameManager.Instance.UpdateGameState((GameState)(GameManager.Instance.State + 1));
        else
            GameManager.Instance.UpdateGameState(GameState.PLAYER_TURN);
    }
    /// <summary>
    /// Checks for if call is valid shoud be performed before calling
    /// </summary>
    public void Call(int _playerNumber)
    {
        int chipsNeededToCall = BetManager.Instance.highestBet - BetManager.Instance.bets[_playerNumber];

        // Player does not have enough/ just enough chips to call so must go all in
        if (chipsNeededToCall >= BetManager.Instance.chips[_playerNumber])
        {
            BetManager.Instance.AddPlayerBet(BetManager.Instance.chips[_playerNumber], _playerNumber);
            BetManager.Instance.AddToPot(BetManager.Instance.chips[_playerNumber]);

            BetManager.Instance.SetPlayerChips(0, _playerNumber);

            UpdateAllIn(_playerNumber);
        }
        else
        {
            BetManager.Instance.chips[_playerNumber] -= chipsNeededToCall;
            BetManager.Instance.bets[_playerNumber] += chipsNeededToCall;
            BetManager.Instance.pot += chipsNeededToCall;

            lastActions[_playerNumber] = Actions.CALL;
        }
    }
    public void PlayerCall()
    {
        Call((int)PlayerOrder.PLAYER);

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

        if (BetManager.Instance.chips[_playerNumber] == 0)
            UpdateAllIn(_playerNumber);
        else
            lastActions[_playerNumber] = Actions.RAISE;
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
            UpdateAllIn(_playerNumber);
        }
        Debug.Log($"Player {_playerNumber} raised by {raiseIncrement * BetManager.Instance.minBet}");
        return raiseIncrement * BetManager.Instance.minBet;
    }
    public void PlayerRaise(int _value)
    {
        Raise((int)PlayerOrder.PLAYER, _value);

        StartCoroutine(MoveToNextStageWithDelay((int)PlayerOrder.PLAYER));
    }
}  

public enum Actions
{
    CHECK, 
    CALL,
    RAISE,
    FOLD,
    ALL_IN, // Needs to be last, not a selectable option just a state for UI updating
    NUM_OF_ACTIONS
}