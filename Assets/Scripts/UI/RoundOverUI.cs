using UnityEngine;
using TMPro;
public class RoundOverUI : MonoBehaviour
{
    public static RoundOverUI Instance;

    public GameObject roundOverPanel;
    public TMP_Text roundDescription;

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
    public void UpdateDescription()
    {
        string text = "";
        int numOfWinners = GameManager.Instance.winners.Count;

        // Iterate throw winners and add to list
        for (int i = 0; i < numOfWinners; i++)
        {
            text += GameManager.GetPlayerName(GameManager.Instance.winners[i]);

            if (i != numOfWinners - 1)
                text += ",";
        }

        text += $" won this round with a {GameManager.GetHandName(GameManager.Instance.winningHandType)}!";

        roundDescription.text = text;
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

        UpdateDescription();
        roundOverPanel.SetActive(true);
    }
}
