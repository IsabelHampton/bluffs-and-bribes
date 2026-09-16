using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState state;
    public static event Action<GameState> OnGameStateChanged;

    public int roundNumber;

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        UpdateGameState(GameState.IN_START_MENU);
    }
    public void UpdateGameState(GameState _state)
    {
        state = _state;
        switch (state)
        {
            case GameState.TIPOFF:
                break;
            case GameState.BRIBE:
                /*if (roundNumber == 0)
                {
                    UpdateGameState(GameState.DEALING);
                }*/
                break;
            case GameState.DEALING:
                break;
            case GameState.PLAYER_TURN:
                break;
            case GameState.ENEMY1_TURN:
                break;
            case GameState.ENEMY2_TURN:
                break;
            case GameState.ENEMY3_TURN:
                break;
            case GameState.REVEAL:
                break;
            case GameState.END_ROUND:
                break;
            default:
                break;
        }
        OnGameStateChanged?.Invoke(state);
    }

}
public enum GameState
{
    IN_START_MENU,
    TIPOFF,
    BRIBE,
    DEALING,
    PLAYER_TURN,
    ENEMY1_TURN,
    ENEMY2_TURN,
    ENEMY3_TURN,
    REVEAL,
    END_ROUND
}
