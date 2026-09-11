using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HandManager : MonoBehaviour
{
    public static HandManager Instance;

    public Card[] playerHand = new Card[2];
    public Card[] e1Hand = new Card[2];
    public Card[] e2Hand = new Card[2];
    public Card[] e3Hand = new Card[2];
    public Card[][] allHands = new Card[(int)PlayerOrder.NUM_OF_PLAYERS][];
    public Card[] flop = new Card[3];
    public Card[] centreCards = new Card[5];
    public Card[] cardsOnTable = new Card[13];

    private List<int> indexesToDrawFrom = new List<int>();
    private List<Card> cardsAvailableToMakeHandFrom = new List<Card>();

    public PlayerHand[] playerHands = new PlayerHand[(int)PlayerOrder.NUM_OF_PLAYERS];

    bool endOfRoundSequenceStarted = false; // Prevent sequence happening multiple times on event trigger? TO DO: Find better fix

    private void UpdateAllHands()
    {
        allHands = new Card[][]{ playerHand, e1Hand, e2Hand, e3Hand};
    }
    private void RevealEndOfRoundHands()
    {
        UpdateAllHands();
        for (int i = 0; i < allHands.Length; i++)
        {
            if (ActionManager.Instance.hasFolded[i])
                continue; // Skip reveal as has folded

            // Reveal cards in hand
            foreach (Card card in allHands[i])
            {
                card.Reveal();
            }
        }
    }

    private void Awake()
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
        switch (GameManager.Instance.state)
        {
            case GameState.DEALING:
                endOfRoundSequenceStarted = false; // Reset 
                // 1st round normal deal cards
                if (GameManager.Instance.roundNumber == 0)
                {
                    RandomDealCards();
                }
                else
                {
                    // TO DO: Cheating mechanics
                }
                RevealCards(playerHand);
                RevealCards(flop);
                UpdateAllHands();
                GameManager.Instance.UpdateGameState(GameState.PLAYER_TURN);
                break;
            case GameState.REVEAL:
                NextCardReveal();
                break;
            case GameState.END_ROUND:
                if (endOfRoundSequenceStarted)
                    break; // Stop doing multiple times 

                endOfRoundSequenceStarted = true; // Now started so tell event thing to stop calling repeatedly
                RevealEndOfRoundHands(); // Calls UpdateHands()

                for (int i = 0; i < (int)PlayerOrder.NUM_OF_PLAYERS; i++)
                {
                    // Check player hasn't folded
                    if (!ActionManager.Instance.hasFolded[i])
                        DetermineCurrentHandRank(allHands[i], i);
                }

                // Sort ranks in descending order
                PlayerHand[] sortedHands = new PlayerHand[playerHands.Length];
                Array.Copy(playerHands, sortedHands, playerHands.Length);
                Array.Sort(sortedHands, ByDescendingHandType);
                Array.Sort(sortedHands, ByDescendingRankPerHandType);

                List<PlayerHand> winningHands = new List<PlayerHand> { sortedHands[0] } ; // 1st element in sorted list is winner
                for (int i = 0; i < sortedHands.Length; i++)
                {
                    if (i == 0)
                        continue; // Already know they are winner and are in list

                    // If next player has same hand and rank add to winners list
                    if (sortedHands[i].handType == sortedHands[i - 1].handType && sortedHands[i].handRank == sortedHands[i - 1].handRank)
                        winningHands.Add(sortedHands[i]);

                    // No more players with same hand, stop checking for winner
                    else
                        break; // No more players with same hand type no need to check
                }

                // Work out split of pot to dish out
                int chipsWon = BetManager.Instance.pot / winningHands.Count;

                // Iterate through list of player hands (in order of player number)
                for (int i = 0; i < playerHands.Length; i++)
                {
                    // If hand is in winning hands list give spilt of pot
                    if (winningHands.Contains(playerHands[i]))
                    {
                        BetManager.Instance.AddPlayerChips(chipsWon, i);// Give player winnings split from pot
                        Debug.Log($"{(PlayerOrder)i} wins {chipsWon} with a {playerHands[i].handType} of rank {playerHands[i].handRank}");
                    }
                    else
                    {
                        if (ActionManager.Instance.hasFolded[i])
                            Debug.Log($"{(PlayerOrder)i} lost as folded");
                        else
                            Debug.Log($"{(PlayerOrder)i} lost with a {playerHands[i].handType} of rank {playerHands[i].handRank}");
                    }
                }
                // Empty pot
                BetManager.Instance.SetPot(0);

                // TO DO: Display next round button

                break;
            default:
                break;
        }
    }
    public void CreateDeck()
    {
        indexesToDrawFrom.Clear();
        for (int i = 0; i < 52; i++)
        {
            indexesToDrawFrom.Add(i);
        }
    }
    public void RandomDealCards()
    {
        CreateDeck();
        for (int i = 0; i < cardsOnTable.Length; i++)
        {
            int cardInt = UnityEngine.Random.Range(0, indexesToDrawFrom.Count);
            cardsOnTable[i].Init(indexesToDrawFrom[cardInt]);
            indexesToDrawFrom.RemoveAt(cardInt);
        }
    }
    public void RevealCards(Card[] _cardsToReveal)
    {
        foreach (Card card in _cardsToReveal)
        {
            card.Reveal();
        }
    }
    public void NextCardReveal()
    {
        bool cardWasRevealed = false;
        foreach (Card card in centreCards)
        {
            if (card.faceDown)
            {
                card.Reveal();
                cardWasRevealed = true;
                break;
            }
        }

        if (cardWasRevealed)
            GameManager.Instance.UpdateGameState(GameState.PLAYER_TURN);
        else
            GameManager.Instance.UpdateGameState(GameState.END_ROUND);
    }
    public void CreateCardPoolForHand(Card[] _currentHand)
    {
        cardsAvailableToMakeHandFrom.Clear();
        foreach (Card card in _currentHand)
        {
            cardsAvailableToMakeHandFrom.Add(card);
        }
        foreach (Card card in centreCards)
        {
            if (card.faceDown == false)
                cardsAvailableToMakeHandFrom.Add(card);
            else
                break; // Stop checking as no more face up cards
        }
    }
    private static int DescendingCardValue(Card _card1, Card _card2)
    {
        if (_card1.Value > _card2.Value)
            return -1;
        else if (_card2.Value > _card1.Value)
            return 1;
        return 0;
    }
    private static int AscendingCardSuit(Card _card1, Card _card2)
    {
        if (_card1.Suit > _card2.Suit)
            return 1;
        else if (_card2.Suit > _card1.Suit)
            return -1;
        return 0;
    }
    /// <summary>
    /// Must be called on array sorted by ascending suit
    /// </summary>
    private static int DescendingValueForAscendingSuit(Card _card1, Card _card2)
    {
        if (_card1.Suit != _card2.Suit)
            return 0; // Don't swap as new suit foud
        return DescendingCardValue(_card1, _card2);
    }
    private static int ByDescendingHandType(PlayerHand _hand1, PlayerHand _hand2)
    {
        if (_hand1.handType > _hand2.handType)
            return -1;
        else if (_hand2.handType > _hand1.handType)
            return 1;
        return 0;
    }
    /// <summary>
    /// Must be called on array sorted by descending rank
    /// </summary>
    private static int ByDescendingRankPerHandType(PlayerHand _hand1, PlayerHand _hand2)
    {
        if (_hand1.handType != _hand2.handType)
            return 0; // Don't swap as new hand type found

        if (_hand1.handRank > _hand2.handRank)
            return -1;
        else if (_hand2.handRank > _hand1.handRank)
            return 1;
        return 0; // Same hand type and rank no swap
    }
    

    public void DetermineCurrentHandRank(Card[] _currentHand, int _playerNumber)
    {
        CreateCardPoolForHand(_currentHand);
        cardsAvailableToMakeHandFrom.Sort(DescendingCardValue);
        int score = 0;
        // Check for straight flush
        score = CheckForSpecificHand(cardsAvailableToMakeHandFrom, _currentHand, FindStraightFlush, CalculateRankForStraightFlush, 5, true);
        if (ApplyScoreIfSpecificHandFound(score, HandType.STRAIGHT_FLUSH, _playerNumber))
            return; // Exit out of method highest hand found

        // Check for four of a kind
        score = CheckForSpecificHand(cardsAvailableToMakeHandFrom, _currentHand, FindFourOfAKind, CalculateRankForFourOfAKind, 4, false); // Can't have multiple 4s of a king in one round
        if (ApplyScoreIfSpecificHandFound(score, HandType.FOUR_OF_A_KIND, _playerNumber))
            return; // Exit out of method highest hand found

        // Check for full house
        score = CheckForSpecificHand(cardsAvailableToMakeHandFrom, _currentHand, FindFullHouse, CalculateRankForFullHouse, 5, true);
        if (ApplyScoreIfSpecificHandFound(score, HandType.FULL_HOUSE, _playerNumber))
            return; // Exit out of method highest hand found

        // Check for flush
        score = CheckForSpecificHand(cardsAvailableToMakeHandFrom, _currentHand, FindFlush, CalculateRankForFlush, 5, true);
        if (ApplyScoreIfSpecificHandFound(score, HandType.FLUSH, _playerNumber))
            return; // Exit out of method highest hand found

        // Check for straight
        score = CheckForSpecificHand(cardsAvailableToMakeHandFrom, _currentHand, FindStraight, CalculateRankForStraight, 5, true);
        if (ApplyScoreIfSpecificHandFound(score, HandType.STRAIGHT, _playerNumber))
            return; // Exit out of method highest hand found

        // Check three of a kind
        score = CheckForSpecificHand(cardsAvailableToMakeHandFrom, _currentHand, FindThreeOfAKind, CalculateRankForThreeOfAKind, 3, true);
        if (ApplyScoreIfSpecificHandFound(score, HandType.THREE_OF_A_KIND, _playerNumber))
            return; // Exit out of method highest hand found

        // Check for two pair
        score = CheckForSpecificHand(cardsAvailableToMakeHandFrom, _currentHand, FindTwoPair, CalculateRankForTwoPair, 4, true);
        if (ApplyScoreIfSpecificHandFound(score, HandType.TWO_PAIR, _playerNumber))
            return; // Exit out of method highest hand found

        // Check for pair
        score = CheckForSpecificHand(cardsAvailableToMakeHandFrom, _currentHand, FindPair, CalculateRankForPair, 2, true);
        if (ApplyScoreIfSpecificHandFound(score, HandType.PAIR, _playerNumber))
            return; // Exit out of method highest hand found

        // Get high card - no check needed as always can have high card
        ApplyScoreIfSpecificHandFound(CalculateRankForHighCard(_currentHand), HandType.HIGH_CARD, _playerNumber);
    }

    private bool ApplyScoreIfSpecificHandFound(int _foundRank, HandType _checkedForHand, int _playerNumber)
    {
        if (_foundRank != 0)
        {
            playerHands[_playerNumber].handType = _checkedForHand;
            playerHands[_playerNumber].handRank = _foundRank;
            return true; // Hand found
        }
        return false;
    }
    public int CheckForSpecificHand(List<Card> _cardsAvailable, Card[] _playerHand, Func<Card[], List<Card>> FindHand, Func<Card[], Card[], int> CalculateHandRank, int _cardsInHand, bool _useRecursion = true)
    {
        List<Card> handToCheck = new List<Card>(FindHand(_cardsAvailable.ToArray()));

        if (handToCheck == null || handToCheck.Count < _cardsInHand)
            return 0; // No two pair found so return 0 for rank to continue search for valid hand

        bool containsPlayerCard = false;
        // Check this hand contains player's cards
        foreach (Card card in handToCheck)
        {
            if (card == _playerHand[0] || card == _playerHand[1])
            {
                containsPlayerCard = true;
            }
        }

        // Highest hand of specified type was only in the flop
        if (!containsPlayerCard && _useRecursion)
        {
            _cardsAvailable.Remove(handToCheck[handToCheck.Count - 1]);
            return CheckForSpecificHand(_cardsAvailable, _playerHand, FindHand, CalculateHandRank, _cardsInHand, _useRecursion);
        }

        return CalculateHandRank(handToCheck.ToArray(), _playerHand);
    }


    public int FindKickerValue(List<Card> _madeHand, Card[] _playerHand)
    {
        if (_madeHand.Contains(_playerHand[0]) && !_madeHand.Contains(_playerHand[1]))
            return _playerHand[1].Value;
        else if (!_madeHand.Contains(_playerHand[0]) && _madeHand.Contains(_playerHand[1]))
            return _playerHand[0].Value;

        // Both cards used to make hand so return lowest value card
        return Mathf.Min(_playerHand[0].Value, _playerHand[1].Value);
    }
    /// <summary>
    /// Must be called on sorted hand
    /// </summary>
    /// <param name="_hand">Must be sorted in value order</param>
    /// <returns></returns>
    public List<Card> FindStraightFlush(Card[] _hand)
    {
        Array.Sort(_hand, AscendingCardSuit); //sort by suit
        Array.Sort(_hand, DescendingValueForAscendingSuit);

        List<Card> straightFlush = new List<Card>();
        int consecutiveCards = 1;
        for (int i = 0; i < _hand.Length; i++)
        {
            if (i == 0)
                continue; // Skip 1st card
            if (_hand[i].Value == _hand[i - 1].Value + 1 && _hand[i].Suit == _hand[i - 1].Suit)
            {
                consecutiveCards++;
                if (consecutiveCards == 5)
                {
                    for (int j = i - 4; j < i + 1; j++)
                    {
                        straightFlush.Add(_hand[j]);
                    }
                    return straightFlush;
                }
            }
            else
                consecutiveCards = 1; // Reset counter ;
            
        }
        return straightFlush;
    }
    public int CalculateRankForStraightFlush(Card[] _straightFlush, Card[] _playerHand)
    {
        Array.Sort(_straightFlush, DescendingCardValue);

        // (Highest straight card value)^2 + kicker
        return (_straightFlush[0].Value * 13) + FindKickerValue(new List<Card>(_straightFlush), _playerHand);
    }
    /// <summary>
    /// Must be called on sorted hand
    /// </summary>
    /// <param name="_hand">Must be sorted in value order</param>
    /// <returns>Value of 4 of a kind</returns>
    public List<Card> FindFourOfAKind(Card[] _hand)
    {
        List<Card> fourOfAKind = new List<Card>();

        int matchingCards = 1; // First card matches itself
        for (int i = 0; i < _hand.Length; i++)
        {
            if (i == 0)
                continue; // Skip first card

            if (_hand[i].Value == _hand[i - 1].Value)
            {
                matchingCards++;
                if (matchingCards == 4)
                {
                    fourOfAKind.Add(_hand[i - 3]);
                    fourOfAKind.Add(_hand[i - 2]);
                    fourOfAKind.Add(_hand[i - 1]);
                    fourOfAKind.Add(_hand[i]);
                    return fourOfAKind;
                }
            } 
            else
                matchingCards = 1; // New value found so reset counter
        }
        return fourOfAKind;
    }
    public int CalculateRankForFourOfAKind(Card[] _fourOfAKind, Card[] _playerHand)
    {
        // (Card value)^2 + kicker
        return (_fourOfAKind[0].Value * 13) + FindKickerValue(new List<Card>(_fourOfAKind), _playerHand);
    }
    /// <summary>
    /// Must be called on sorted hand
    /// </summary>
    /// <param name="_hand">Must be sorted in descending value order</param>
    /// <returns></returns>
    public List<Card> FindFullHouse(Card[] _hand)
    {
        List<Card> threeOfAKind = new List<Card>();
        List<Card> pair = new List<Card>();
        List<Card> fullHouse = new List<Card>();
        int matchingCards = 1; // Matching with itself

        for (int i = 0; i < _hand.Length; i++)
        {
            if (i == 0)
                continue; // Skip 1st one
            if (_hand[i].Value == _hand[i - 1].Value)
            {
                matchingCards++;
                // If found pair AND no pair found yet
                if (matchingCards == 2 && pair.Count == 0)
                {
                    pair.Add(_hand[i - 1]);
                    pair.Add(_hand[i]);
                }
                else if (matchingCards == 3)
                {
                    threeOfAKind.Add(_hand[i - 2]);
                    threeOfAKind.Add(_hand[i - 1]);
                    threeOfAKind.Add(_hand[i]);
                    // Check if found pair AND if pair is part of this 3 of a kind
                    if (pair.Count > 0 && pair[0].Value == _hand[i].Value)
                        pair.Clear();
                }

                // Check if full house has now been found
                if (threeOfAKind.Count == 3 && pair.Count == 2)
                {
                    fullHouse = threeOfAKind;
                    fullHouse.Add(pair[0]);
                    fullHouse.Add(pair[1]);
                    return fullHouse;
                }
            }
            else
            {
                matchingCards = 1; // Reset counter
            }
        }
        return fullHouse;
    }
    public int CalculateRankForFullHouse(Card[] _fullHouse, Card[] _playerHand)
    {
        // 3 of a kind value cubed + pair value squared + kicker
        return (_fullHouse[0].Value * 13 * 13) + (_fullHouse[3].Value * 13) + FindKickerValue(new List<Card>(_fullHouse), _playerHand);
    }
    /// <summary>
    /// Must be called on sorted hand
    /// </summary>
    /// <param name="_hand">Must be sorted in value suit</param>
    /// <returns></returns>
    public List<Card> FindFlush(Card[] _hand)
    {
        Array.Sort(_hand, AscendingCardSuit); // Sort cards by suits
        Array.Sort(_hand, DescendingValueForAscendingSuit);

        List<Card> flush = new List<Card>();
        int matchingSuit = 1;
        for (int i = 0; i < _hand.Length; i++)
        {
            if (i == 0) // Skip first card
                continue;
            if (_hand[i].Suit == _hand[i - 1].Suit)
            {
                matchingSuit++;
                if (matchingSuit == 5)
                {
                    for (int j = i - 4; j < i + 1; j++)
                    {
                        flush.Add(_hand[j]);
                    }
                    return flush;
                }
            }
            else
                matchingSuit = 1; // Reset counter
        }
        return flush;
    }
    public int CalculateRankForFlush(Card[] _flush, Card[] _playerHand)
    {
        int suit = _flush[0].Suit;

        if (_playerHand[0].Suit == suit && _playerHand[1].Suit != suit)
            return (int)Mathf.Pow(_playerHand[0].Value, 2) + FindKickerValue(new List<Card>(_flush), _playerHand);

        if (_playerHand[1].Suit == suit && _playerHand[0].Suit != suit)
            return (int)Mathf.Pow(_playerHand[1].Value, 2) + FindKickerValue(new List<Card>(_flush), _playerHand);

        // Both cards must be in flush
        return (Mathf.Max(_playerHand[0].Value, _playerHand[1].Value) * 13) + FindKickerValue(new List<Card>(_flush), _playerHand);
    }
    /// <summary>
    ///
    /// </summary>
    /// <param name="_hand">Must be sorted in descending value order</param>
    /// <returns>List of cards making up straight</returns>
    public List<Card> FindStraight(Card[] _hand)
    {
        List<Card> straight = new List<Card>();
        int consecutiveCards = 1; // 1st card starts order
        for (int i = 0; i < _hand.Length; i++)
        {
            if (i == 0 || _hand[i].Value == _hand[i-1].Value)
                continue; // Skip 1st card or cards of same value
            if (_hand[i].Value == _hand[i - 1].Value + 1)
            {
                consecutiveCards++;
                if (consecutiveCards == 5)
                {
                    for (int j = i - 4; j < i + 1; j++)
                    {
                        straight.Add(_hand[j]);
                    }
                    return straight;
                }

            }
            else
                consecutiveCards = 1; // Reset counter
        }
        return straight;
    }
    public int CalculateRankForStraight(Card[] _straight, Card[] _playerHand)
    {
        Array.Sort(_straight, DescendingCardValue);

        return (_straight[0].Value * 13) + FindKickerValue(new List<Card>(_straight), _playerHand);

    }
    /// <summary>
    /// Must be called on sorted hand
    /// </summary>
    /// <param name="_hand">Must be sorted in value order</param>
    /// <returns>Value of 3 of a kind</returns>
    public List<Card> FindThreeOfAKind(Card[] _hand)
    {
        List<Card> threeOfAKind = new List<Card>();

        int matchingCards = 1; // First card matches itself
        for (int i = 0; i < _hand.Length; i++)
        {
            if (i == 0)
                continue; // Skip first card

            if (_hand[i].Value == _hand[i - 1].Value)
            {
                matchingCards++;
                if (matchingCards == 3)
                {
                    threeOfAKind.Add(_hand[i - 2]);
                    threeOfAKind.Add(_hand[i - 1]);
                    threeOfAKind.Add(_hand[i]);
                    return threeOfAKind;
                }
            }
            else
                matchingCards = 1; // New value found so reset counter
        }
        return threeOfAKind;
    }
    public int CalculateRankForThreeOfAKind(Card[] _threeOfAKind, Card[] _playerHand)
    {
        return (_threeOfAKind[0].Value * 13) + FindKickerValue(new List<Card>(_threeOfAKind), _playerHand);
    }

    /// <summary>
    /// Must be called on sorted hand 
    /// </summary>
    /// <param name="_hand">Must be sorted in value order</param>
    /// <returns>Returns list of cards making up 2 pair found</returns>
    public List<Card> FindTwoPair(Card[] _hand)
    {
        List<Card> twoPairHand = new List<Card>();
        int pairsFound = 0;
        int matchingCards = 1; // First card matches with itself
        for (int i = 0; i < _hand.Length; i++)
        {
            if (i == 0)
                continue; // Skip first card check
            if (_hand[i].Value == _hand[i - 1].Value)
            {
                matchingCards++;
                if (matchingCards == 2) // Pair found
                {
                    twoPairHand.Add(_hand[i - 1]);
                    twoPairHand.Add(_hand[i]);
                    pairsFound++;
                    if (pairsFound == 2)
                        return twoPairHand;
                }
            }
            else
                matchingCards = 1; // Reset counter
        }
        return twoPairHand; // No valid two pair found, this will be returned empty and picked up by later checks
    }
    public int CalculateRankForTwoPair(Card[] _twoPair, Card[] _playerHand)
    {
        return (int)(Mathf.Max(_twoPair[0].Value, _twoPair[2].Value) * 13 * 13) +
            (Mathf.Min(_twoPair[0].Value, _twoPair[2].Value) * 13) +
            FindKickerValue(new List<Card>(_twoPair), _playerHand);
    }
    /// <summary>
    /// Must be called on sorted hand
    /// </summary>
    /// <param name="_hand">Must be sorted in value order</param>
    /// <returns>Value of pair</returns>
    public List<Card> FindPair(Card[] _hand)
    {
        List<Card> pair = new List<Card>();

        int matchingCards = 1; // First card matches itself
        for (int i = 0; i < _hand.Length; i++)
        {
            if (i == 0)
                continue; // Skip first card

            if (_hand[i].Value == _hand[i - 1].Value)
            {
                matchingCards++;
                if (matchingCards == 2)
                {
                    pair.Add(_hand[i - 1]);
                    pair.Add(_hand[i]);
                    return pair;
                }
            }
            else
                matchingCards = 1; // New value found so reset counter
        }
        return pair;
    }
    public int CalculateRankForPair(Card[] _pair, Card[] _playerHand)
    {
        return (_pair[0].Value * 13) + FindKickerValue(new List<Card>(_pair), _playerHand);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_hand">Array of cards with length 2</param>
    /// <returns></returns>
    public int CalculateRankForHighCard(Card[] _hand)
    {
        return (int)(Mathf.Max(_hand[0].Value, _hand[1].Value) * 13) + Mathf.Min(_hand[0].Value, _hand[1].Value);
    }
}

public enum HandType
{
    HIGH_CARD,
    PAIR,
    TWO_PAIR,
    THREE_OF_A_KIND,
    STRAIGHT,
    FLUSH,
    FULL_HOUSE,
    FOUR_OF_A_KIND, 
    STRAIGHT_FLUSH,
    HAND_TYPES
}

public struct PlayerHand
{
    public HandType handType;
    public int handRank;
}