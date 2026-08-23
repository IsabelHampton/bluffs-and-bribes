using UnityEngine;
using UnityEngine.UI;

public enum Suit
{
    HEART,
    DIAMOND,
    CLUB,
    SPADE
}
public class Card : MonoBehaviour
{
    // preset values assigned in inpector - never change
    [SerializeField] private CardImages cardImages;
    [SerializeField] private Sprite faceDownSprite;

    public int Value;
    public int Suit;
    [SerializeField] private bool faceDown;
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
    public void Reveal()
    {
        faceDown = false;
        displayedImage.sprite = faceUpSprite;
    }
}
