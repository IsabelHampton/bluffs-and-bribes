using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;


public class PlayerRaiseUI : MonoBehaviour
{
    public GameObject raiseMenu;
    public TMP_Text chipsDisplay;
    public Button increaseBetButton;
    public Button decreaseBetButton;
    public Button betButton;
    public Button raiseButton;

    int chipsToBet = 0;

    private void Awake()
    {
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
        raiseMenu.SetActive(true);
        raiseButton.enabled = false; // Menu open can't press again
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
        raiseButton.enabled = true; // Menu closed offer to press again press again
    }

}
