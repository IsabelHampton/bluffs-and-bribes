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
    public int Value;
    public int Suit;
    public bool faceDown;

    private Image displayImage;
}
