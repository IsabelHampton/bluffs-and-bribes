using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BribeManager : MonoBehaviour
{
    public static BribeManager Instance;

    private int[] bribes = new int[(int)PlayerOrder.NUM_OF_PLAYERS];

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
        ResetBribes();

        if (GameManager.Instance.state == GameState.BRIBE)
        {

        }
        
    }
    public void SetBribeAmount(int _bribeAmount, int _playerNumber)
    {
        bribes[_playerNumber] = _bribeAmount;
    }
}
