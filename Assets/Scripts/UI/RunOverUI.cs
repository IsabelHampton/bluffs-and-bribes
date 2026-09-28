using UnityEngine;
using TMPro;

public class RunOverUI : MonoBehaviour
{
    public static RunOverUI Instance;

    [SerializeField] public GameObject runOverPanel; 

    public TMP_Text roundsCompletedDisplay;
    public TMP_Text maxChipsDisplay;
    public TMP_Text opponentsElimatedDisplay;
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
        if (GameManager.Instance.State == GameState.END_OF_RUN_MENU)
        {
            OpenEndOfRunMenu();
        }
    }

        public void UpdateTextDisplayed()
    {
        roundsCompletedDisplay.text = $"Rounds completed: {GameManager.Instance.RoundNumber}";
        maxChipsDisplay.text = $"Max chips achieved: {GameManager.Instance.MaxChipsThisRun}";
        opponentsElimatedDisplay.text = $"Opponents eliminated: {GameManager.Instance.OpponentsElimatedThisRun}";
    }
    public void OnNewRun()
    {
        runOverPanel.SetActive(false); //hide this menu
        // Restart run 
        GameManager.Instance.ResetRunStats();
        GameManager.Instance.UpdateGameState(GameState.TIPOFF);
    }
    public void QuitToStartMenu()
    {
        StartMenuUI.Instance.SetUp();
        GameManager.Instance.UpdateGameState(GameState.IN_START_MENU);
    }
    public void OpenEndOfRunMenu()
    {
        // Vibrate phone to show end of run
        VibrationManager.Instance.VibratePhone();

        runOverPanel.SetActive(true);
        UpdateTextDisplayed();
    }
}
