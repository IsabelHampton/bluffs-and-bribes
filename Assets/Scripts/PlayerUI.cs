using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private Button checkButton;
    [SerializeField] private Button callButton;
    [SerializeField] private Button raiseButton;
    [SerializeField] private Button foldButton;
    [SerializeField] private Button tipOffButton;
    [SerializeField] private GameObject tipOffMenu;
    private int viewingTipOff;
    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnStateChanged;
    }

    private void GameManagerOnStateChanged(GameState _state)
    {
        UpdateButtonInteractability(_state == GameState.PLAYER_TURN);
    }

    private void UpdateButtonInteractability(bool _isInteractable)
    {
        checkButton.interactable = _isInteractable;
        callButton.interactable = _isInteractable;
        raiseButton.interactable = _isInteractable;
        foldButton.interactable = _isInteractable;
    }
    public void OpenTipoffMenu()
    {

    }
    public void HideTipOffMenu()
    {
        tipOffMenu.SetActive(false);
        viewingTipOff = 0;
    }
    public void NextTipOff()
    {

    }
    public void PreviousTipOff()
    {

    }


}
