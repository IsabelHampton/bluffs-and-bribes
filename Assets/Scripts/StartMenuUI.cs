using UnityEngine;

public class StartMenuUI : MonoBehaviour
{
    public GameObject newRunConfirmationPanel;
    public GameObject startMenuPanel;
    public GameObject tablePanel;
    public GameObject tipoffPanel;

    public HandManager handManager;
    private void Start()
    {
        GameManager.OnGameStateChanged += GameManagerOnStateChanged;
        SetUp();
        
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnStateChanged;
    }
    private void GameManagerOnStateChanged(GameState _state)
    {
        switch (GameManager.Instance.state)
        {
            case GameState.IN_START_MENU:
                SetUp();
                break;
            case GameState.TIPOFF:
                break;
            case GameState.DEALING:
                break;
            case GameState.PLAYER_TURN:
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

    /// <summary>
    /// Set up which panels are hidden
    /// </summary>
    private void SetUp()
    {
        tablePanel.SetActive(false);
        newRunConfirmationPanel.SetActive(false);
        tipoffPanel.SetActive(false);
        startMenuPanel.SetActive(true);
    }
    public void OnNewRun()
    {
        // TO DO: Add check if previous run exists, if not ignore confirm 

        // Ask confirmation as this will override previous run
        newRunConfirmationPanel.SetActive(true);

    }
    public void OnContinueRun()
    {

    }
    public void OnQuit()
    {
        Application.Quit();
    }
    public void OnNewRunConfirmation()
    {
        // Update game state
        GameManager.Instance.UpdateGameState(GameState.TIPOFF);

        // Set up UI
        startMenuPanel.SetActive(false);
        newRunConfirmationPanel.SetActive(false);

        tablePanel.SetActive(true);
    }
    public void OnNewRunCancellation()
    {
        newRunConfirmationPanel.SetActive(false);
    }
}
