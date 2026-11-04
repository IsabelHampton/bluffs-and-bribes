using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    public static TutorialManager Instance;
    public bool isInTutorial { get; private set; }
    public int tutorialRoundNumber { get; private set; }
    /// <summary>
    /// Player cards, opponent 1's cards, opponent 2's cards. opponent 3's cards, centre cards
    /// </summary>

    public CardSaveData[] round0CardsOnTable = new CardSaveData[13] // Also set in Inspector
    {
        new CardSaveData(14, (int)Suits.DIAMOND, false), //Player
        new CardSaveData(14, (int)Suits.HEART, false),
        new CardSaveData(8, (int)Suits.SPADE), // Opponent 1
        new CardSaveData(6, (int)Suits.HEART),
        new CardSaveData(3, (int)Suits.CLUB), // Opponent 2
        new CardSaveData(7, (int)Suits.DIAMOND),
        new CardSaveData(12, (int)Suits.DIAMOND), // Opponent 3
        new CardSaveData(13, (int)Suits.CLUB),
        new CardSaveData(8, (int)Suits.DIAMOND), // Centre cards
        new CardSaveData(11, (int)Suits.CLUB),
        new CardSaveData(12, (int)Suits.DIAMOND),
        new CardSaveData(14, (int)Suits.HEART),
        new CardSaveData(14, (int)Suits.SPADE, false)
    };

    private void Awake()
    {
        Instance = this;

       if (FindAnyObjectByType<SaveManager>().ReadSaveData()) // Using method instead of Instance to avoid null ref in Awake
        {
            if (SaveManager.Instance.saveData.runsPlayed == 0)
                PlayTutorial();
        }
       // No save file
        else
        {
            Debug.Log("No save file detected");
            PlayTutorial();
        }
    }
    void PlayTutorial()
    {
        isInTutorial = true;
        // Set up cards and bets 

        // End of tutorial
        //isInTutorial = false;
    }

    public bool TutorialSkipsTipOff()
    {
        return isInTutorial;
    }
    public bool TutorialSkipsBribe() 
    {
        return isInTutorial;
    }

}
