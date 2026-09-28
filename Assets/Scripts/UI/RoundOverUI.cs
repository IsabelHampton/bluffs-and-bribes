using UnityEngine;
using TMPro;
public class RoundOverUI : MonoBehaviour
{
    public static RoundOverUI Instance;

    [SerializeField] public GameObject roundOverPanel;

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
        if (GameManager.Instance.State == GameState.END_OF_ROUND_MENU)
        {
            OpenEndOfRoundMenu();
        }
    }
    public void OnNewRound()
    {
        roundOverPanel.SetActive(false); // hide this menu
        GameManager.Instance.UpdateGameState(GameState.TIPOFF); // all bet resetting handled in BetManager
    }
    public void OpenEndOfRoundMenu()
    {
        // Vibrate phone to show end of round
        VibrationManager.Instance.HeavyVibrate();

        roundOverPanel.SetActive(true);
    }
}
