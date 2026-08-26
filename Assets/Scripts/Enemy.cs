using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int enemyNumber;
    [SerializeField] private GameState playState;

    void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnStateChanged;
        playState = (GameState)(enemyNumber + 5);
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnStateChanged;
    }

    private void GameManagerOnStateChanged(GameState _state)
    {
        if (_state == playState)
        {

        }
    }
}