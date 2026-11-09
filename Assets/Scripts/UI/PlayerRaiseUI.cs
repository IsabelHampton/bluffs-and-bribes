using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;


public class PlayerRaiseUI : MonoBehaviour
{
    public static PlayerRaiseUI Instance;

    public GameObject raiseMenu;
    public TMP_Text chipsDisplay;
    public Button increaseBetButton;
    public Button decreaseBetButton;
    public Button betButton;
    public Button raiseButton;

    int chipsToBet = 0;

    int[][] riggedRaiseValuesPerTutorialRound =
    {
        new int[] {0, 15, 30 },
        new int[] { },
        new int[] { }
    };

    public int GetRaiseAmount(int _roundNumber, int _turnNumber)
    {
        return riggedRaiseValuesPerTutorialRound[_roundNumber][_turnNumber];
    }

    private void Awake()
    {
        Instance = this;

        SetUp();
        raiseMenu.SetActive(false); //initially hidden
        GameManager.OnGameStateChanged += GameManagerOnStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnStateChanged;
    }

    private void GameManagerOnStateChanged(GameState _state)
    {
        if (raiseMenu.activeInHierarchy)
            raiseMenu.SetActive(false);
    }

    public void SetUp()
    {
        decreaseBetButton.interactable = false;
        increaseBetButton.interactable = true;
        chipsToBet = BetManager.Instance.minBet;
        chipsDisplay.text = Convert.ToString(chipsToBet);
    }

    public void OnRaiseClicked()
    {
        SetUp();
        UpdateButtonInteractability();
        raiseMenu.SetActive(true);

        raiseButton.interactable = false; // Menu open can't press again

        //Hide check area to prevent overlap
        DoubleTapArea.Instance.HideArea();
    }

    public void OnIncreaseBet()
    {
        chipsToBet += BetManager.Instance.minBet;
        chipsDisplay.text = Convert.ToString(chipsToBet);
        UpdateButtonInteractability();
    }
    public void OnDecreaseBet()
    {
        chipsToBet -= BetManager.Instance.minBet;
        chipsDisplay.text = Convert.ToString(chipsToBet);
        UpdateButtonInteractability();
    }

    public void UpdateButtonInteractability()
    {
        int chipsNeededToCall = BetManager.Instance.highestBet - BetManager.Instance.bets[(int)PlayerOrder.PLAYER];
        int chipsAvailableToRaise = BetManager.Instance.chips[(int)PlayerOrder.PLAYER] - chipsNeededToCall;

        if (chipsToBet + BetManager.Instance.minBet > chipsAvailableToRaise)
            increaseBetButton.interactable = false;
        else
            increaseBetButton.interactable = true;

        if (chipsToBet - BetManager.Instance.minBet < BetManager.Instance.minBet)
            decreaseBetButton.interactable = false;
        else
            decreaseBetButton.interactable = true;

        // Further check for tutorial to force raise values
        if (TutorialManager.Instance.isInTutorial)
        {
            // Only enable submit button on raise menu if equal to set amount for tutorial
            if (chipsToBet == GetRaiseAmount(TutorialManager.Instance.tutorialRoundNumber, TutorialManager.Instance.playerTurnsThisRound))
            {
                betButton.interactable = true;
            }
            else
            {
                betButton.interactable = false;
            }
        }
    }

    public void OnConfirmRaiseAmount()
    {
        raiseMenu.SetActive(false);
        ActionManager.Instance.PlayerRaise(chipsToBet);
    }
    public void OnCancelRaise()
    {
        chipsToBet = 0;
        chipsDisplay.text = BetManager.Instance.minBet.ToString();
        raiseMenu.SetActive(false);
        raiseButton.interactable = true; // Menu closed offer to press again press again

        // Re-enable removed double tap area if needed
        if (!TutorialManager.Instance.isInTutorial && ActionManager.Instance.CanCheck((int)PlayerOrder.PLAYER))
            StartCoroutine(DoubleTapArea.Instance.ShowAreaWithDelay(0.2f));
    }

}
