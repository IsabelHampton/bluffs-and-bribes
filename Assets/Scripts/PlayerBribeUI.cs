using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PlayerBribeUI : MonoBehaviour
{

    public GameObject bribeMenu;

    public TMP_Text bribeDisplay;
    public Button increaseBribeButton;
    public Button decreaseBribeButton;
    public Button bribeButton;
    public Button submitBribeButton;

    int chipsToBribe = 0;

    private void Awake()
    {
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
        UpdateButtonToAllowBetting(); // Enables / disables bribe button
        bribeMenu.SetActive(false);
    }

    public void SetUp()
    {
        decreaseBribeButton.interactable = false;
        increaseBribeButton.interactable = true;
        chipsToBribe = BetManager.Instance.minBet;
        bribeDisplay.text = Convert.ToString(chipsToBribe);
    }

    public void OnBribeClicked()
    {
        bribeMenu.SetActive(true);
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

    public void UpdateButtonToAllowBetting()
    {
        if (GameManager.Instance.state == GameState.BRIBE && BetManager.Instance.chips[(int)PlayerOrder.PLAYER] > BetManager.Instance.minBet * 2)
            bribeButton.interactable = true;
        else
            bribeButton.interactable = false;
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
        bribeMenu.SetActive(false);

        BribeManager.Instance.SetBribeAmount(chipsToBribe, (int)PlayerOrder.PLAYER);

        GameManager.Instance.UpdateGameState(GameState.DEALING); // Move stage on
    }

    public void SkipBribe()
    {
        GameManager.Instance.UpdateGameState(GameState.DEALING); // Move stage on
    }
}
