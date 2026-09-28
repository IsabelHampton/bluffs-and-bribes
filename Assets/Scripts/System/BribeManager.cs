using UnityEngine;
public class BribeManager : MonoBehaviour
{
    public static BribeManager Instance;

    private int[] bribes = new int[(int)PlayerOrder.NUM_OF_PLAYERS];
    public bool bribeStateReached = false;
    public bool BribeSetUpComplete;

    public void ResetBribes()
    {
        bribes = new int[(int)PlayerOrder.NUM_OF_PLAYERS];
    }

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
        if (GameManager.Instance.State != GameState.BRIBE)
            bribeStateReached = false; // will allow to be retriggered next time state is reached

        else if (GameManager.Instance.State == GameState.BRIBE && !bribeStateReached) // Event triggered multiple times added check to prevent this
        {
            bribeStateReached = true;

            ResetBribes();
            // TO DO: Add check if bribes are acceptable this round
            for (int i = 0; i < (int)PlayerOrder.NUM_OF_PLAYERS; i++)
            {
                // Skip player as this is done with UI
                if (i == (int)PlayerOrder.PLAYER)
                    continue;

                // Place bribes for enemies
                int avaialbleBribeIncrements = Mathf.Max(0, (BetManager.Instance.chips[i] / BetManager.Instance.minBet) - 3 );

                if (avaialbleBribeIncrements == 0)
                    continue; // Skip this enemy as does not have enough chips to bribe

                int bribeIncrement = Random.Range(0, avaialbleBribeIncrements);
                SetBribeAmount(BetManager.Instance.minBet * bribeIncrement, i);
            }

            // Bribe set up complete - attempt to move to dealing 
            BribeSetUpComplete = true;
            AttemptMoveToDealing();

        }
        
    }
    public void AttemptMoveToDealing()
    {
        if (GameManager.Instance.State != GameState.DEALING && PlayerBribeUI.Instance.PlayerSelectedOption && BribeSetUpComplete)
            GameManager.Instance.UpdateGameState(GameState.DEALING);
    }
    public void SetBribeAmount(int _bribeAmount, int _playerNumber)
    {
        bribes[_playerNumber] = _bribeAmount;
        BetManager.Instance.AddPlayerChips(-_bribeAmount, _playerNumber);
    }
    public int HighestBribePlayerNumber() 
    {
        int highestBribe = 0;
        int highestPlayer = 0;
        for (int i = 0; i < (int)PlayerOrder.NUM_OF_PLAYERS; i++)
        {
            if (bribes[i] > highestBribe)
            {
                highestBribe = bribes[i];
                highestPlayer = i;
            }
        }

        Debug.Log($"Highest bribe is {highestBribe} and highest player is {(PlayerOrder)(highestPlayer)}");

        if (highestBribe == 0)
            return -1; // No highest bribe

        return highestPlayer;
    }
}
