using UnityEngine;
using UnityEngine.UI;
using System;
public enum Suits
{
    HEART,
    DIAMOND,
    CLUB,
    SPADE,
    NUM_OF_SUITS
}

[Serializable]
public class Card : MonoBehaviour
{
    // preset values assigned in inpector - never change
    [SerializeField] private CardImages cardImages;
    [SerializeField] private Sprite faceDownSprite;

    public int Value = 0;
    public int Suit = 0;
    [SerializeField] public bool faceDown;
    public Image displayedImage;

    [SerializeField] private Sprite faceUpSprite;

    public void Init (int _value, int _suit, bool _faceDown = true)
    {
        Value = _value;
        Suit = _suit;
        faceDown = _faceDown;

        faceUpSprite = cardImages.cardSprites[((Suit * 13) + (Value - 2))];

        if (faceDown)
            displayedImage.sprite = faceDownSprite;
        else
            displayedImage.sprite = faceUpSprite;
    }

    public void Init(Card _card)
    {
        Init(_card.Value, _card.Suit, _card.faceDown); 
    }

    public void Init(int _cardValue, bool _faceDown = true)
    {
        Value = (_cardValue % 13) + 2; // +2 as card values start from 2
        Suit = (int)(_cardValue / 13);
        faceDown = _faceDown;

        faceUpSprite = cardImages.cardSprites[_cardValue];

        if (faceDown)
            displayedImage.sprite = faceDownSprite;
        else
            displayedImage.sprite = faceUpSprite;
    }
    public void Init(CardSaveData _data)
    {
        Init(_data.Value, _data.Suit, _data.FaceDown);
    }
    public void Reveal()
    {
        faceDown = false;
        displayedImage.sprite = faceUpSprite;
    }

    public void Hide()
    {
        faceDown = true;
        displayedImage.sprite = faceDownSprite;
    }

}
