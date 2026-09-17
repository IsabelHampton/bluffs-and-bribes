using UnityEngine;

public class BetManager : MonoBehaviour
{
    public static BetManager Instance;
    public int pot = 0;
    public static int startAmount = 100;

    public int[] bets  = {0, 0, 0, 0 };
    public int[] chips = { startAmount, startAmount, startAmount, startAmount };
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
        switch (GameManager.Instance.State)
        {
            case GameState.DEALING:
                pot = 0; // Zero out pot for new round
                ResetBets();
                break;
            case GameState.REVEAL: // 1st turn of round
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

    public void SetPlayerChips(int _value, int _playerNumber)
    {
        chips[_playerNumber] = _value;
        ChipsUI.Instance.UpdateElements();
    }

    public void AddPlayerChips(int _value, int _playerNumber)
    {
        chips[_playerNumber] += _value;
        ChipsUI.Instance.UpdateElements();
    }

    public void SetPot(int _value)
    {
        pot = _value;
        ChipsUI.Instance.UpdateElements();
    }

    public void AddToPot(int _value)
    {
        pot += _value;
        ChipsUI.Instance.UpdateElements();
    }

    public void SetPlayerBet(int _value, int _playerNumber)
    {
        bets[_playerNumber] = _value;
        ChipsUI.Instance.UpdateElements();
    }

    public void AddPlayerBet(int _value, int _playerNumber)
    {
        bets[_playerNumber] += _value;
        ChipsUI.Instance.UpdateElements();
    }

    public bool IsPlayerBankrupt()
    {
        return chips[(int)PlayerOrder.PLAYER] == 0;
    }
    public int BankruptPlayers()
    {
        int bankruptPlayers = 0;
        foreach (int amount in chips)
        {
            if (amount == 0)
                bankruptPlayers++;
        }
        return bankruptPlayers;
    }

    public void IntroduceNewPlayers()
    {
        for (int i = 0; i < chips.Length; i++)
        {
            if (chips[i] == 0)
            {
                SetPlayerChips(startAmount, i);
                ChipsUI.Instance.UpdateElements();
            }
                
        }
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