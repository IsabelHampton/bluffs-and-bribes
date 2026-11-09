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

    [Header("Tutorial Values")]
    private int playerTurnsThisRound;

    private Actions[][] opponent1TutorialActions = new Actions[][] {
        new Actions [] { Actions.CHECK, Actions.CALL, Actions.FOLD },
        new Actions [] { },
        new Actions[]{ }
    };
    private Actions[][] opponent2TutorialActions = new Actions[][] {
        new Actions [] { Actions.CHECK, Actions.FOLD},
        new Actions [] { },
        new Actions[]{ }
    };
    private Actions[][] opponent3TutorialActions = new Actions[][] {
        new Actions [] { Actions.CHECK, Actions.CALL, Actions.CALL },
        new Actions [] { },
        new Actions[]{ }
    };


    public void UpdateAllIn(int _playerNumber)
    {
        isAllIn[_playerNumber] = true;
        UpdateLastActions(_playerNumber, Actions.ALL_IN);
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

            case GameState.OPPONENT1_TURN:
                EnemyTurn((int)PlayerOrder.OPPONENT1);
                break;

            case GameState.OPPONENT2_TURN:
                EnemyTurn((int)PlayerOrder.OPPONENT2);
                break;

            case GameState.OPPONENT3_TURN:
                EnemyTurn((int)PlayerOrder.OPPONENT3);
                break;

            default:
                break;
        }
    }

    public void ResetPlayerStates()
    {
        if (GameManager.Instance.ignoreResets)
            return;

        hasFolded = new bool[(int)PlayerOrder.NUM_OF_PLAYERS];
        isAllIn = new bool[(int)PlayerOrder.NUM_OF_PLAYERS];
        lastActions = new Actions[(int)PlayerOrder.NUM_OF_PLAYERS];
        foldedPlayers = 0;
    }

    public void EnemyTurn(int _playerNumber)
    {
        if (TurnNeedsSkipping(_playerNumber))
            MoveToNextStageWithoutDelay(_playerNumber);

        // Check if in tutorial - complete set action
        if (TutorialManager.Instance.isInTutorial)
        {
            CompleteTutorialRiggedActionsForRound(_playerNumber);
            return;
        }
        else
            TakeTurn(_playerNumber);
    }
    public void TakeTurn(int _playerNumber)
    {
        DeterminePossibleActionsForTurn(_playerNumber);

        int actionNumberIndex = Random.Range(0, possibleActions.Count);
        PerformAction(possibleActions[actionNumberIndex], _playerNumber); 
    }

    public void PerformAction(int _actionNumber, int _playerNumber) 
    {
        switch (_actionNumber)
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

        Debug.Log($"Player {(PlayerOrder)_playerNumber} performed action {(Actions)_actionNumber}");

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
        UpdateLastActions(_playerNumber, Actions.FOLD);
    }
    public void PlayerFold()
    {
        Fold((int)PlayerOrder.PLAYER);

        MoveToNextStageWithoutDelay((int)PlayerOrder.PLAYER);
    }
    public IEnumerator MoveToNextStageWithDelay(int _playerNumber)
    {
        yield return new WaitForSecondsRealtime(1.0f);

        if (_playerNumber == (int)PlayerOrder.NUM_OF_PLAYERS - 1) // if on last player
        {
            HandleIfRoundCanEnd();
        }
        else
            GameManager.Instance.UpdateGameState((GameState)(GameManager.Instance.State + 1));
    }
    public void MoveToNextStageWithoutDelay(int _playerNumber)
    {
        if (_playerNumber == (int)PlayerOrder.NUM_OF_PLAYERS - 1) // On last player
        {
            HandleIfRoundCanEnd();
        }
        else
            GameManager.Instance.UpdateGameState((GameState)(GameManager.Instance.State + 1));
    }
    public void Check(int _playerNumber)
    {
        UpdateLastActions(_playerNumber, Actions.CHECK);
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
        {
            // Increment counter if in tutorial
            playerTurnsThisRound++;

            GameManager.Instance.UpdateGameState((GameState)(GameManager.Instance.State + 1));
        }

        // Round can't end - return play to player
        else
        {
            GameManager.Instance.UpdateGameState(GameState.PLAYER_TURN);
        }
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
            UpdateLastActions(_playerNumber, Actions.CALL);

            BetManager.Instance.AddPlayerBet(chipsNeededToCall, _playerNumber);
            BetManager.Instance.AddToPot(chipsNeededToCall);
            BetManager.Instance.AddPlayerChips(-chipsNeededToCall, _playerNumber);
        }
    }
    public void PlayerCall()
    {
        Call((int)PlayerOrder.PLAYER);

        MoveToNextStageWithoutDelay((int)PlayerOrder.PLAYER);
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
            UpdateLastActions(_playerNumber, Actions.RAISE);
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

        MoveToNextStageWithoutDelay((int)PlayerOrder.PLAYER);
    }

    public void UpdateLastActions(int _playerNumber, Actions _action)
    {
        lastActions[_playerNumber] = _action;
        ActionUI.Instance.DisplayActionUI(_playerNumber, _action);
    }

    public void CompleteTutorialRiggedActionsForRound(int _playerNumber)
    {
        switch (_playerNumber)
        {
            case (int)PlayerOrder.OPPONENT1:
                PerformRiggedAction(opponent1TutorialActions, _playerNumber);
                //PerformAction((int)opponent1TutorialActions[TutorialManager.Instance.tutorialRoundNumber][playerTurnsThisRound], _playerNumber);
                break;
            case (int)PlayerOrder.OPPONENT2:
                PerformRiggedAction(opponent2TutorialActions, _playerNumber);
                //PerformAction((int)opponent2TutorialActions[TutorialManager.Instance.tutorialRoundNumber][playerTurnsThisRound], _playerNumber);
                break; 
            case (int)PlayerOrder.OPPONENT3:
                PerformRiggedAction(opponent3TutorialActions, _playerNumber);
                //PerformAction((int)opponent3TutorialActions[TutorialManager.Instance.tutorialRoundNumber][playerTurnsThisRound], _playerNumber);
                break;
            default:
                break;
        }
    }

    public void PerformRiggedAction(Actions[][] tutorialActions, int _playerNumber)
    {
        // Check there is an action assigned for this player this round (e.g. if folded later values might be null)
        if (playerTurnsThisRound < tutorialActions[TutorialManager.Instance.tutorialRoundNumber].Length)
            PerformAction((int)tutorialActions[TutorialManager.Instance.tutorialRoundNumber][playerTurnsThisRound], _playerNumber);
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