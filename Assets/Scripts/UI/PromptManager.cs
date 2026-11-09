using UnityEngine;
using TMPro;

public class PromptManager : MonoBehaviour
{
    string[] prompts = new string [10];
    public void CreatePrompts()
    {
        prompts = new string[] {
        "Great start, you've been dealt a pair of aces! Press the <i>Handbook</i> icon in the top-left to view all the possible hands. \n For now, let's <b>Check</b> to begin with...",
        $"Looks like everyone else has <b>Checked</b> this turn, so no money has now been bet this far. The next card has been revealed, and you can now make a 3 of a Kind! <b>Raise</b> {PlayerRaiseUI.Instance.GetRaiseAmount(0, 1)} chips and see which players will match you...",
        $"Player 2 has <b>folded</b>, that means they are out of this round and cannot win any of the money in the pot. But, Player 1 and 3 have called your bet! With the last card on the river, you can make a 4 of a kind! Bet {PlayerRaiseUI.Instance.GetRaiseAmount(0, 2)} chips to see if any other players will <b>Call</b>"
        };
    }

    public GameObject PromptPanel;
    public TMP_Text PromptText;

    int promptNumber;

    void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnStateChanged;

        HidePrompt(); // Hide prompt by default
    }

    private void Start()
    {
        CreatePrompts(); // In start to avoid null ref on PlayerRaiseUI
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnStateChanged;
    }
    private void GameManagerOnStateChanged(GameState _state)
    {
        // Check if in tutorial
        if (!TutorialManager.Instance.isInTutorial)
            return;

        if (_state == GameState.PLAYER_TURN)
        {
            ShowPrompt(promptNumber);
        }
        else
        {
            HidePrompt();
        }
    }
    public void UpdatePromptText(int _promptNumber)
    {
        PromptText.text = prompts[_promptNumber];
    }
    public void ShowPrompt(int _promptNumber)
    {
        UpdatePromptText(_promptNumber);

        PromptPanel.SetActive(true);

        promptNumber++; // Increment ready to display next prompt
    }
    public void HidePrompt()
    {
        PromptPanel.SetActive(false);
    }
}
