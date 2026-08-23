using UnityEngine;
using UnityEngine.UI;

public class HandManager : MonoBehaviour
{
    public Card[] playerHand = new Card[2];
    public Card[] e1Hand = new Card[2];
    public Card[] e2Hand = new Card[2];
    public Card[] e3Hand = new Card[2];
    public Card[] centreCards = new Card[5];

    public Image[] cardHandDisplays;
    public Image[] centreCardsDisplays;
    public void PlayTutorial()
    {

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
