using System;
using UnityEngine;
using TMPro;

public class ChipsUI : MonoBehaviour
{
    public static ChipsUI Instance;

    public TMP_Text playerChipsDisplay;
    public TMP_Text e1ChipsDisplay;
    public TMP_Text e2ChipsDisplay;
    public TMP_Text e3ChipsDisplay;
    public TMP_Text potChipsDisplay;
    void Awake()
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
        switch (GameManager.Instance.state)
        {
            case GameState.BRIBE:
                UpdateElements();
                break;
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
            case GameState.END_ROUND:
                UpdateElements();
                break;
            default:
                break;
        }
    }
    public void UpdateElements()
    {
        playerChipsDisplay.text = Convert.ToString(BetManager.Instance.chips[(int)PlayerOrder.PLAYER]);
        e1ChipsDisplay.text = Convert.ToString(BetManager.Instance.chips[(int)PlayerOrder.ENEMY1]);
        e2ChipsDisplay.text = Convert.ToString(BetManager.Instance.chips[(int)PlayerOrder.ENEMY2]);
        e3ChipsDisplay.text = Convert.ToString(BetManager.Instance.chips[(int)PlayerOrder.ENEMY3]);
        potChipsDisplay.text = Convert.ToString(BetManager.Instance.pot);
    }
}
