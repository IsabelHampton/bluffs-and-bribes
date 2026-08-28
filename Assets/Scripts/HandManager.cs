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
    public Card[] flop = new Card[3];
    public Card[] centreCards = new Card[5];
    public Card[] cardsOnTable = new Card[13];

    private List<int> indexesToDrawFrom = new List<int>();
    private List<Card> cardsAvailableToMakeHandFrom = new List<Card>();

    public int[] handRanks = new int[(int)PlayerOrder.NUM_OF_PLAYERS];
    public int[] handValue = new int[(int)PlayerOrder.NUM_OF_PLAYERS];

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

                GameManager.Instance.UpdateGameState(GameState.PLAYER_TURN);
                break;
            case GameState.REVEAL:
                NextCardReveal();
                break;
            case GameState.END_ROUND:
                // REMOVE
                centreCards[0].Init(14, (int)Suit.HEART, false);
                centreCards[1].Init(14, (int)Suit.CLUB, false);
                centreCards[2].Init(12, (int)Suit.HEART, false);
                centreCards[3].Init(12, (int)Suit.DIAMOND, false);
                centreCards[4].Init(9, (int)Suit.HEART, false);

                playerHand[0].Init(10, (int)Suit.DIAMOND, false);
                playerHand[1].Init(10, (int)Suit.CLUB, false);

                List<Card> testCards = new List<Card> { centreCards[0], centreCards[1], centreCards[2], centreCards[3], centreCards[4], playerHand[0], playerHand[1], };
                testCards.Sort(DescendingCardValue);
                foreach(Card card in testCards)
                    Debug.Log($"{card.Value} {(Suit)card.Suit}");
                //int rank = CheckForTwoPair(testCards, playerHand);
                int rank = CheckForSpecificHand(testCards, playerHand, FindTwoPair, CalculateRankForTwoPair, 4, true);
                Debug.Log($"Two pair rank from test {rank}");
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

    public void DetermineCurrentHandRank(Card[] _currentHand)
    {
        CreateCardPoolForHand(_currentHand);
        cardsAvailableToMakeHandFrom.Sort(DescendingCardValue);

    }
    /// <summary>
    ///
    /// </summary>
    /// <param name="_hand">2 cards player has</param>
    /// <returns>Higher value of card player is using for hand</returns>
    public int HighestPlayerOwnedCardInHand(Card[] _hand)
    {
        int returnValue = 0;
        foreach (Card card in _hand)
            if (card == playerHand[0] && playerHand[0].Value > returnValue)
            {
                returnValue = playerHand[0].Value;
            }
            else if (card == playerHand[1] && playerHand[1].Value > returnValue)
            {
                returnValue = playerHand[1].Value;
            }
        return returnValue;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_cardsAvailable">Needs to ve sorted in descending order by card value</param>
    /// <param name="_playerHand"></param>
    /// <param name="FindHand"></param>
    /// <param name="CalculateHandRank"></param>
    /// <param name="_cardsInHand"></param>
    /// <param name="_useRecursion"></param>
    /// <returns></returns>

    public int CheckForSpecificHand(List<Card> _cardsAvailable, Card[] _playerHand, Func<Card[], List<Card>> FindHand, Func<Card[], int> CalculateHandRank, int _cardsInHand, bool _useRecursion = true)
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

        return CalculateHandRank(handToCheck.ToArray());
    }
    /// <summary>
    /// Must be called on sorted hand
    /// </summary>
    /// <param name="_hand">Must be sorted in value order</param>
    /// <returns></returns>
    public int FindStraightFlush(Card[] _hand)
    {

        return 0;
    }
    public int CalculateStraightFlushRank(Card[] _hand)
    {
        return 0;
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
    public int CalculateRankForFourOfAKind(Card[] _fourOfAKind)
    {
        return _fourOfAKind[0].Value;
    }
    /// <summary>
    /// Must be called on sorted hand
    /// </summary>
    /// <param name="_hand">Must be sorted in descending value order</param>
    /// <returns></returns>
    public List<Card> CheckForFullHouse(Card[] _hand)
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
    public int CalculateRankForFullHouse(Card[] _fullHouse)
    {
        // 3 of a kind value squared + pair value
        return (int)( Mathf.Pow(_fullHouse[0].Value, 2) + _fullHouse[3].Value);
    }
    /// <summary>
    /// Must be called on sorted hand
    /// </summary>
    /// <param name="_hand">Must be sorted in value suit</param>
    /// <returns></returns>
    public List<Card> CheckForFlush(Card[] _hand)
    {
        Array.Sort(_hand, AscendingCardSuit); // Sort cards by suit

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
            return _playerHand[0].Value;

        if (_playerHand[1].Suit == suit && _playerHand[0].Suit != suit)
            return _playerHand[1].Value;

        // Both cards must be in flush
        return (int)Mathf.Max(_playerHand[0].Value, _playerHand[1].Value);
    }
    /// <summary>
    ///
    /// </summary>
    /// <param name="_hand">Must be sorted in descending value order</param>
    /// <returns>List of cards making up straight</returns>
    public List<Card> CheckForStraight(Card[] _hand)
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
    public int CalculateRankForStraight(Card[] _straight)
    {
        Array.Sort(_straight, DescendingCardValue);
        return _straight[0].Value;
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
    public int CalculateRankForThreeOfAKind(Card[] _threeOfAKind)
    {
        return _threeOfAKind[0].Value;
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
    public int CalculateRankForTwoPair(Card[] _twoPair)
    {
        return (int)(Mathf.Pow(Mathf.Max(_twoPair[0].Value, _twoPair[2].Value), 2) + Mathf.Min(_twoPair[0].Value, _twoPair[2].Value));
    }
    /// <summary>
    /// Must be called on sorted hand
    /// </summary>
    /// <param name="_hand">Must be sorted in value order</param>
    /// <returns>Value of pair</returns>
    public List<Card> CheckForPair(Card[] _hand)
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
    public int CalculateRankForPair(Card[] _pair)
    {
        return _pair[0].Value;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_hand">Array of cards with length 2</param>
    /// <returns></returns>
    public int CalculateRankForHighCard(Card[] _hand)
    {
        return Mathf.Max(_hand[0].Value, _hand[1].Value);
    }
}

enum HandRank
{
    HIGH_CARD,
    PAIR,
    TWO_PAIR,
    THREE_OF_A_KIND,
    STRAIGHT,
    FLUSH,
    FULL_HOUSE,
    FOUR_OF_A_KIND, 
    STRAIGHT_FLUSH
}