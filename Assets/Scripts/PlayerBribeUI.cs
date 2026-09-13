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
        if (bribeMenu.activeInHierarchy)
            bribeMenu.SetActive(false);
    }

    public void SetUp()
    {
        decreaseBribeButton.interactable = false;
        increaseBribeButton.interactable = true;
        chipsToBribe = BetManager.Instance.minBet;
        bribeDisplay.text = Convert.ToString(chipsToBribe);
    }

    public void OnRaiseClicked()
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

    public void UpdateButtonInteractability()
    {
        if (GameManager.Instance.state == GameState.BRIBE && BetManager.Instance.chips[(int)PlayerOrder.PLAYER] > BetManager.Instance.minBet * 2)
            bribeButton.interactable = true;
        else
            bribeButton.interactable = false;
    }

    public void OnConfirmBribeAmount()
    {
        bribeMenu.SetActive(false);
        // TO DO: Submit bribe funcitonality
        BribeManager.Instance.SetBribeAmount(chipsToBribe, (int)PlayerOrder.PLAYER);
    }
}
