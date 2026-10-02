using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerBribeUI : MonoBehaviour
{
    public static PlayerBribeUI Instance; 
    public GameObject bribeMenu;

    public TMP_Text bribeDisplay;
    public Button increaseBribeButton;
    public Button decreaseBribeButton;
    public Button submitBribeButton;
    public Button cancelBribeButton;

    int chipsToBribe = 0;
    public bool PlayerSelectedOption = false;

    private void Awake()
    {
        Instance = this;

        SetUp();
        bribeMenu.SetActive(false); //initially hidden
        GameManager.OnGameStateChanged += GameManagerOnStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnStateChanged;
    }

    private void GameManagerOnStateChanged(GameState _state)
    {
        if (GameManager.Instance.State == GameState.BRIBE)
        {
            SetUp();
            UpdateButtonToAllowBribing(); // Enables / disables bribe button
            UpdateButtonInteractability(); //Updates increase/ decrease button availability

            bribeMenu.SetActive(true); // Show menu
        }
        else
        {
            PlayerSelectedOption = false; // Reset for next round
            bribeMenu.SetActive(false);
        }
    }

    public void SetUp()
    {
        decreaseBribeButton.interactable = false;
        increaseBribeButton.interactable = true;
        chipsToBribe = BetManager.Instance.minBet;
        bribeDisplay.text = Convert.ToString(chipsToBribe);
    }

    public void OnIncreaseBribe()
    {
        chipsToBribe += BetManager.Instance.minBet;
        bribeDisplay.text = Convert.ToString(chipsToBribe);
        UpdateButtonInteractability();
    }
    public void OnDecreaseBribe()
    {
        chipsToBribe -= BetManager.Instance.minBet;
        bribeDisplay.text = Convert.ToString(chipsToBribe);
        UpdateButtonInteractability();
    }

    public void UpdateButtonToAllowBribing()
    {
        // If bribing allowed
        if (GameManager.Instance.State == GameState.BRIBE && BetManager.Instance.chips[(int)PlayerOrder.PLAYER] > BetManager.Instance.minBet * 2)
        {
            submitBribeButton.enabled = true;
            cancelBribeButton.enabled = true;
        }

        else
        {
            submitBribeButton.enabled = false;
            increaseBribeButton.enabled = false;
            decreaseBribeButton.enabled = false;
            cancelBribeButton.enabled = true;
        }
    }

    public void UpdateButtonInteractability()
    {
        // Player shouldn't able to leave themsleves without at least 3x the min bet
        if (chipsToBribe + BetManager.Instance.minBet > BetManager.Instance.chips[(int)PlayerOrder.PLAYER] - ( BetManager.Instance.minBet * 3) ) 
            increaseBribeButton.interactable = false;
        else
            increaseBribeButton.interactable = true;


        if (chipsToBribe - BetManager.Instance.minBet < BetManager.Instance.minBet)
            decreaseBribeButton.interactable = false;
        else
            decreaseBribeButton.interactable = true;
    }

    public void OnConfirmBribeAmount()
    {
        BribeManager.Instance.SetBribeAmount(chipsToBribe, (int)PlayerOrder.PLAYER);

        bribeMenu.SetActive(false);

        AttemptMoveToDealing();
    }
    public void AttemptMoveToDealing()
    {
        PlayerSelectedOption = true;
        BribeManager.Instance.AttemptMoveToDealing();
    }

    public void OnCancelBribe()
    {
        bribeMenu.SetActive(false);
        AttemptMoveToDealing();
    }
}
