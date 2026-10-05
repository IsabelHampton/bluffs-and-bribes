using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public static PlayerUI Instance; 

    [SerializeField] private Button checkButton;
    [SerializeField] private Button callButton;
    [SerializeField] private Button raiseButton;
    [SerializeField] private Button foldButton;
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
        UpdateButtonInteractability(GameManager.Instance.State == GameState.PLAYER_TURN); //events fire in wrong order here so needs to reference instance
    }
    private void UpdateButtonInteractability(bool _isInteractable)
    {
        bool canCallOrFold = ActionManager.Instance.CanCallOrFold((int)PlayerOrder.PLAYER);

        checkButton.interactable = _isInteractable && BetManager.Instance.bets[(int)(PlayerOrder.PLAYER)] == BetManager.Instance.highestBet;
        callButton.interactable = _isInteractable && canCallOrFold; // Calling if bet is higher than ownded chips just results in all-in
        raiseButton.interactable = _isInteractable && BetManager.Instance.chips[(int)(PlayerOrder.PLAYER)] >= (BetManager.Instance.highestBet - BetManager.Instance.bets[(int)PlayerOrder.PLAYER]) + BetManager.Instance.minBet;
        foldButton.interactable = _isInteractable && canCallOrFold;
    }



}
