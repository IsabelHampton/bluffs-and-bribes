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
    public void OnNewRun()
    {
        roundOverPanel.SetActive(false); // hide this menu
        GameManager.Instance.UpdateGameState(GameState.TIPOFF); // all bet resetting handled in BetManager
    }
    public void OpenEndOfRoundMenu()
    {
        roundOverPanel.SetActive(true);
    }
}
