using UnityEngine;

public class BetManager : MonoBehaviour
{
    public static BetManager Instance;
    public int pot;

    public int[] bets  = { };
    public int[] chips = { };
    public int highestBet;

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
            case GameState.BRIBE:
                break;
            case GameState.DEALING:
                break;
            case GameState.PLAYER_TURN: // 1st turn of round
                ResetBets();
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
    }
    private void ResetBets()
    {
        bets = new int[]{ 0, 0, 0, 0 };
        highestBet = 0;
    }
}

public enum PlayerOrder
{
    PLAYER,
    ENEMY1,
    ENEMY2,
    ENEMY3
}