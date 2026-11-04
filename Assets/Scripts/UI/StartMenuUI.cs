using UnityEngine;
using UnityEngine.UI;

public class StartMenuUI : MonoBehaviour
{
    public static StartMenuUI Instance;

    [Header("Panels")]
    public GameObject newRunConfirmationPanel;
    public GameObject startMenuPanel;
    public GameObject tablePanel;
    public GameObject tipoffPanel;
    public GameObject endOfRunPanel;
    public GameObject endOfRoundPanel;
    public GameObject shufflingPanel;
    public GameObject tutorialPanel;

    [Header("Tutorial Prompts")]
    public GameObject newRunPrompt;

    [Header("Buttons")]
    public Button continueRunButton;
    private void Start()
    {
        Instance = this;
        GameManager.OnGameStateChanged += GameManagerOnStateChanged;
        SetUp();
        
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnStateChanged;
    }
    private void GameManagerOnStateChanged(GameState _state)
    {
        switch (GameManager.Instance.State)
        {
            case GameState.IN_START_MENU:
                SetUp();
                UpdateContinueRunButton();
                break;
            case GameState.TIPOFF:
                break;
            case GameState.DEALING:
                break;
            case GameState.PLAYER_TURN:
                break;
            case GameState.OPPONENT1_TURN:
                break;
            case GameState.OPPONENT2_TURN:
                break;
            case GameState.OPPONENT3_TURN:
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
    public void SetUp()
    {
        tablePanel.SetActive(false);
        newRunConfirmationPanel.SetActive(false);
        tipoffPanel.SetActive(false);
        endOfRunPanel.SetActive(false);
        endOfRoundPanel.SetActive(false);
        shufflingPanel.SetActive(false);
        tutorialPanel.SetActive(false);

        startMenuPanel.SetActive(true);

        SetUpTutorialUI();
    }

    public void SetUpTutorialUI()
    {
        if (TutorialManager.Instance.isInTutorial)
            newRunPrompt.SetActive(true);
        else
            newRunPrompt.SetActive(false);
    }
    public void OnNewRun()
    {
        // Ask confirmation as this will override previous run
        if (TutorialManager.Instance.isInTutorial || !continueRunButton.interactable)
            OnNewRunConfirmation();
        else
            newRunConfirmationPanel.SetActive(true);

    }
    public void OnContinueRun()
    {
        SaveManager.Instance.LoadInSaveData();

        // Set up UI
        startMenuPanel.SetActive(false);

        tablePanel.SetActive(true);
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

    public void UpdateContinueRunButton()
    {
        if (SaveManager.Instance == null)
            continueRunButton.interactable = false;
        else
            continueRunButton.interactable = SaveManager.Instance.ReadSaveData();
    }
}
