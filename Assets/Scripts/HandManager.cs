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
            int cardInt = Random.Range(0, indexesToDrawFrom.Count);
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
    private static int CompareCardsByValue(Card _card1, Card _card2)
    {
        if (_card1.Value > _card2.Value)
            return 1;
        else if (_card2.Value > _card1.Value)
            return -1;
        return 0;
    }

    public void DetermineCurrentHandRank(Card[] _currentHand)
    {
        CreateCardPoolForHand(_currentHand);
        cardsAvailableToMakeHandFrom.Sort(CompareCardsByValue);

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
    /// Must be called on sorted hand
    /// </summary>
    /// <param name="_hand">Must be sorted in value order</param>
    /// <returns></returns>
    public int ContainsStraightFlush(Card[] _hand)
    {

        return 0;
    }
    /// <summary>
    /// Must be called on sorted hand
    /// </summary>
    /// <param name="_hand">Must be sorted in value order</param>
    /// <returns>Value of 4 of a kind</returns>
    public int ContainsFourOfAKind(Card[] _hand)
    {
        int matchingCards = 1; // First card matches itself
        for (int i = 0; i < _hand.Length; i++)
        {
            if (i == 0)
                continue; // Skip first card

            if (_hand[i].Value == _hand[i - 1].Value)
            {
                matchingCards++;
                if (matchingCards == 4)
                    return _hand[i].Value;
            } 
            else
                matchingCards = 1; // New value found so reset counter
        }
        return 0;
    }
    /// <summary>
    /// Must be called on sorted hand
    /// </summary>
    /// <param name="_hand">Must be sorted in value order</param>
    /// <returns></returns>
    public int ContainsFullHouse(Card[] _hand)
    {
        return 0;
    }
    /// <summary>
    /// Must be called on sorted hand
    /// </summary>
    /// <param name="_hand">Must be sorted in value suit</param>
    /// <returns></returns>
    public int ContainsFlush(Card[] _hand)
    {
        return 0;
    }
    /// <summary>
    /// Must be called on sorted hand
    /// </summary>
    /// <param name="_hand">Must be sorted in value order</param>
    /// <returns>Highest value in straight</returns>
    public int ContainsStraight(Card[] _hand)
    {
        return 0;
    }
    /// <summary>
    /// Must be called on sorted hand
    /// </summary>
    /// <param name="_hand">Must be sorted in value order</param>
    /// <returns>Value of 3 of a kind</returns>
    public int ContainsThreeOfAKind(Card[] _hand)
    {
        int matchingCards = 1; // First card matches itself
        for (int i = 0; i < _hand.Length; i++)
        {
            if (i == 0)
                continue; // Skip first card

            if (_hand[i].Value == _hand[i - 1].Value)
            {
                matchingCards++;
                if (matchingCards == 3)
                    return _hand[i].Value;
            }
            else
                matchingCards = 1; // New value found so reset counter
        }
        return 0;
    }
    /// <summary>
    /// Must be called on sorted hand
    /// </summary>
    /// <param name="_hand">Must be sorted in value order</param>
    /// <returns>Returns values of pairs</returns>
    public int[] ContainsTwoPair(Card[] _hand)
    {
        return new int[2];
    }
    /// <summary>
    /// Must be called on sorted hand
    /// </summary>
    /// <param name="_hand">Must be sorted in value order</param>
    /// <returns>Value of pair</returns>
    public int ContainsPair(Card[] _hand)
    {
        int matchingCards = 1; // First card matches itself
        for (int i = 0; i < _hand.Length; i++)
        {
            if (i == 0)
                continue; // Skip first card

            if (_hand[i].Value == _hand[i - 1].Value)
            {
                matchingCards++;
                if (matchingCards == 2)
                    return _hand[i].Value;
            }
            else
                matchingCards = 1; // New value found so reset counter
        }
        return 0;
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="_hand">Array of cards with length 2</param>
    /// <returns></returns>
    public int PlayerHighCard(Card[] _hand)
    {
        return Mathf.Max(_hand[0].Value, _hand[1].Value);
    }
}
