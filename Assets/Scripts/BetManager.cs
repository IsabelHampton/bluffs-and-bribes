using UnityEngine;

public class BetManager : MonoBehaviour
{
    public static BetManager Instance;
    public int pot = 0;

    public int[] bets  = {0, 0, 0, 0 };
    public int[] chips = {100, 100, 100, 100 };
    public int highestBet = 0;
    public int minBet = 5;

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
            case GameState.PLAYER_TURN: // 1st turn of round
                ResetBets();
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
    ENEMY3,
    NUM_OF_PLAYERS
}