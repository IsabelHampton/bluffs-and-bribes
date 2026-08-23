using UnityEngine;

public class StartMenuUI : MonoBehaviour
{
    public GameObject newRunConfirmationPanel;
    public GameObject startMenuPanel;
    public GameObject tablePanel;

    private void Start()
    {
        SetUp();
    }
    /// <summary>
    /// Set up which panels are hidden
    /// </summary>
    private void SetUp()
    {
        tablePanel.SetActive(false);
        newRunConfirmationPanel.SetActive(false);
        startMenuPanel.SetActive(true);
    }
    public void OnNewRun()
    {
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
        startMenuPanel.SetActive(false);
        tablePanel.SetActive(true);
    }
    public void OnNewRunCancellation()
    {
        newRunConfirmationPanel.SetActive(false);
    }
}
