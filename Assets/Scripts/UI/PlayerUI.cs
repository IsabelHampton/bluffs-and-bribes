using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Instance;

    [Header("Action Buttons")]
    [SerializeField] private Button checkButton;
    [SerializeField] private Button callButton;
    [SerializeField] private Button raiseButton;
    [SerializeField] private Button foldButton;

    [Header("Tutorial Prompts")]
    private int playerTurns;

    public GameObject tutorialPrompts;
    int promptNumber;
    private void Awake()
    {
        Instance = this;
        GameManager.OnGameStateChanged += GameManagerOnStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnStateChanged;
    }

    private void GameManagerOnStateChanged(GameState _state)
    {
        // Reset counter for rigging tutorial buttons
        if (GameManager.Instance.State == GameState.DEALING && TutorialManager.Instance.isInTutorial)
            playerTurns = 0;

        if (GameManager.Instance.State != GameState.PLAYER_TURN)
        {
            DisableAllButtons();
            return;
        }

        // Is player's turn

        if (!TutorialManager.Instance.isInTutorial) // If not in tutorial complete normal checks
            UpdateButtonInteractability(); 

        // Player in tutorial - force actions
        else
        {
            switch(TutorialManager.Instance.tutorialRoundNumber) 
            { 
                case 0:
                    UpdateTutorialButtonInteractibilityRound0();
                    break;
                case 1:
                    break;
                case 2:
                    break;
                default:
                    break;
            }

            playerTurns++; // Only need tracking for tutorial
        }
    }
    private void UpdateButtonInteractability()
    {
        bool canCallOrFold = ActionManager.Instance.CanCallOrFold((int)PlayerOrder.PLAYER);

        checkButton.interactable =BetManager.Instance.bets[(int)(PlayerOrder.PLAYER)] == BetManager.Instance.highestBet;
        callButton.interactable = canCallOrFold; // Calling if bet is higher than ownded chips just results in all-in
        raiseButton.interactable = BetManager.Instance.chips[(int)(PlayerOrder.PLAYER)] >= (BetManager.Instance.highestBet - BetManager.Instance.bets[(int)PlayerOrder.PLAYER]) + BetManager.Instance.minBet;
        foldButton.interactable = canCallOrFold;
    }

    private void UpdateTutorialButtonInteractibilityRound0()
    {
        switch (playerTurns)    
        {
            case 0:
                ForceCheckButton();
                break;
            case 1:
                ForceRaiseButton();
                break;
            case 2:
                ForceRaiseButton();
                break;
            case 3:
                ForceCallButton();
                break;
            default:
                break;
        }
    }

    public void DisableAllButtons()
    {
        checkButton.interactable = false;
        callButton.interactable = false;
        raiseButton.interactable = false;
        foldButton.interactable = false;
    }
    public void ForceCheckButton()
    {
        checkButton.interactable = true;
        callButton.interactable = false;
        raiseButton.interactable = false;
        foldButton.interactable = false;
    }
    public void ForceRaiseButton()
    {
        checkButton.interactable = false;
        callButton.interactable = false;
        raiseButton.interactable = true;
        foldButton.interactable = false;
    }
    public void ForceCallButton()
    {
        checkButton.interactable = false;
        callButton.interactable = true;
        raiseButton.interactable = false;
        foldButton.interactable = false;
    }
    public void ForceFoldButton()
    {
        checkButton.interactable = false;
        callButton.interactable = false;
        raiseButton.interactable = false;
        foldButton.interactable = true;
    }


}
