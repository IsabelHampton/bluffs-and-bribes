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
    [SerializeField] private Button nextTipOffButton;
    [SerializeField] private Button previousTipOffButton;

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
        UpdateButtonInteractability(GameManager.Instance.state == GameState.PLAYER_TURN); //events fire in wrong order here so needs to reference instance
    }
    private void UpdateButtonInteractability(bool _isInteractable)
    {
        checkButton.interactable = _isInteractable && BetManager.Instance.bets[(int)(PlayerOrder.PLAYER)] == BetManager.Instance.highestBet;
        callButton.interactable = _isInteractable;
        raiseButton.interactable = _isInteractable;
        foldButton.interactable = _isInteractable;
    }
    public void OpenTipoffMenu()
    {
        tipOffMenu.SetActive(true);
        viewingTipOff = 0;
        TipOffManager.Instance.unlockedTipOffs[0].SetActive(true);
        previousTipOffButton.interactable = false;
        if (TipOffManager.Instance.unlockedTipOffs.Count > 1)
            nextTipOffButton.interactable = true;
        else
            nextTipOffButton.interactable = false;
    }
    public void HideTipOffMenu()
    {
        tipOffMenu.SetActive(false);
    }
    public void NextTipOff()
    {
        previousTipOffButton.interactable = true;

        TipOffManager.Instance.unlockedTipOffs[viewingTipOff].SetActive(false);
        viewingTipOff++;
        TipOffManager.Instance.unlockedTipOffs[viewingTipOff].SetActive(true);

        if (viewingTipOff == TipOffManager.Instance.unlockedTipOffs.Count)
            nextTipOffButton.interactable = false;
    }
    public void PreviousTipOff()
    {
        nextTipOffButton.interactable = true;

        TipOffManager.Instance.unlockedTipOffs[viewingTipOff].SetActive(false);
        viewingTipOff--;
        TipOffManager.Instance.unlockedTipOffs[viewingTipOff].SetActive(true);

        if (viewingTipOff == 0)
            previousTipOffButton.interactable = false;
    }

    public void OnCheck()
    {
        GameManager.Instance.UpdateGameState(GameState.ENEMY1_TURN);
    }


}
