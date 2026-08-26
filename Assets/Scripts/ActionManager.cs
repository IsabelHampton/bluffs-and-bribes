using System;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public static ActionManager Instance;

    public bool[] hasFolded = new bool[4];

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
        switch (_state)
        {
            case GameState.PLAYER_TURN:
                if (hasFolded[0])
                    GameManager.Instance.UpdateGameState(GameState.ENEMY1_TURN);
                break;
            case GameState.ENEMY1_TURN:
                if (hasFolded[1])
                    GameManager.Instance.UpdateGameState(GameState.ENEMY2_TURN);
                else
                    TakeTurn();
                break;
            case GameState.ENEMY2_TURN:
                if (hasFolded[2])
                    GameManager.Instance.UpdateGameState(GameState.ENEMY2_TURN);
                else
                    TakeTurn();
                break;
            case GameState.ENEMY3_TURN:
                if (hasFolded[3])
                {
                    // Check if pay should got back to player or reveal

                }
                else
                    TakeTurn();
                break;
            case GameState.REVEAL:
                break;
            case GameState.END_ROUND:
                break;
            default:
                break;
        }
    }

    private void TakeTurn()
    {
        
    }

    public void Check()
    {
        GameManager.Instance.UpdateGameState(GameManager.Instance.state + 1);
    }
    public void Fold()
    {
        switch (GameManager.Instance.state)
        {
            case GameState.PLAYER_TURN:
                hasFolded[0] = true;
                GameManager.Instance.UpdateGameState(GameManager.Instance.state + 1);
                break;
            case GameState.ENEMY1_TURN:
                hasFolded[1] = true;
                GameManager.Instance.UpdateGameState(GameManager.Instance.state + 1);
                break;
            case GameState.ENEMY2_TURN:
                hasFolded[2] = true;
                GameManager.Instance.UpdateGameState(GameManager.Instance.state + 1);
                break;
            case GameState.ENEMY3_TURN:
                hasFolded[3] = true;
                // Check if everyone has matched any outstanding bets if not repeat rounds
                bool roundCanEnd = true;

                for (int i = 0; i < BetManager.Instance.bets.Length; i++)
                {
                    if( hasFolded[i] == false && BetManager.Instance.bets[i] < BetManager.Instance.highestBet)
                    {
                        roundCanEnd = false;
                    }
                }

                if (roundCanEnd)
                    GameManager.Instance.UpdateGameState(GameManager.Instance.state + 1);
                else
                    GameManager.Instance.UpdateGameState(GameState.PLAYER_TURN);
                break;
            default:
                break;
        }
        
    }
    public void Call()
    {
        switch (GameManager.Instance.state)
        {
            case GameState.PLAYER_TURN:
                break;
            case GameState.ENEMY1_TURN:
                break;
            case GameState.ENEMY2_TURN:
                break;
            case GameState.ENEMY3_TURN:
                break;
            default:
                break;
        }
    }
    public void Raise(int _value)
    {

    }
}  