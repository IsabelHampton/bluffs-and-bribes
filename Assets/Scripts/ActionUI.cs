using System;
using UnityEngine;
using TMPro;

public class ActionUI : MonoBehaviour
{
    public TMP_Text playerActionDisplay;
    public TMP_Text e1ActionDisplay;
    public TMP_Text e2ActionDisplay;
    public TMP_Text e3ActionDisplay;
    void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnStateChanged;
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnStateChanged;
    }
    private void GameManagerOnStateChanged(GameState _state)
    {
        switch (GameManager.Instance.State)
        {
            case GameState.PLAYER_TURN:
                UpdateElements();
                break;
            case GameState.ENEMY1_TURN:
                UpdateElements();
                break;
            case GameState.ENEMY2_TURN:
                UpdateElements();
                break;
            case GameState.ENEMY3_TURN:
                UpdateElements();
                break;
            case GameState.REVEAL:
                UpdateElements();
                break;
            default:
                break;
        }
    }
    public void UpdateElements()
    {
        playerActionDisplay.text = Convert.ToString(ActionManager.Instance.lastActions[(int)PlayerOrder.PLAYER]);
        e1ActionDisplay.text = Convert.ToString(ActionManager.Instance.lastActions[(int)PlayerOrder.ENEMY1]);
        e2ActionDisplay.text = Convert.ToString(ActionManager.Instance.lastActions[(int)PlayerOrder.ENEMY2]);
        e3ActionDisplay.text = Convert.ToString(ActionManager.Instance.lastActions[(int)PlayerOrder.ENEMY3]);
    }
}
