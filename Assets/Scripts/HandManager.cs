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
    public Card[] centreCards = new Card[5];
    public Card[] cardsOnTable = new Card[13];

    public Image[] cardHandDisplays;
    public Image[] centreCardsDisplays;

    private List<int> indexesToDrawFrom = new List<int>();

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
        if (GameManager.Instance.state == GameState.DEALING)
        {
            // 1st round normal deal cards
            if (GameManager.Instance.roundNumber == 0)
            {
                RandomDealCards();
            }
            else
            {
                // TO DO: Cheating mechanics
            }
            GameManager.Instance.UpdateGameState(GameState.PLAYER_TURN);
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
            cardsOnTable[i].Init(indexesToDrawFrom[cardInt], false);
            indexesToDrawFrom.RemoveAt(cardInt);
        }
    }

    public void RevealCards()
    {

    }
    public void ShowFlop()
    {
        for (int i = 0; i < 2; i++)
        {
            centreCards[i].Reveal();
        }
    }
    public void ShowTurn()
    {
        centreCards[3].Reveal();
    }
    public void ShowRiver()
    {
        centreCards[4].Reveal();
    }
}
