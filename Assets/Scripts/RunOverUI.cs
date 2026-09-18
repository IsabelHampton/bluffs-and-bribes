using UnityEngine;
using TMPro;

public class RunOverUI : MonoBehaviour
{
    [SerializeField] public GameObject runOverPanel; 

    public TMP_Text roundsCompletedDisplay;
    public TMP_Text maxChipsDisplay;
    public TMP_Text opponentsElimatedDisplay;

    public void UpdateTextDisplayed()
    {
        roundsCompletedDisplay.text = $"Rounds completed: {GameManager.Instance.RoundNumber}";
        maxChipsDisplay.text = $"Max chips achieved: {GameManager.Instance.RoundNumber}";
        opponentsElimatedDisplay.text = $"Opponents eliminated: {GameManager.Instance.RoundNumber}";
    }
    public void OnNewRun()
    {

    }
    public void QuitToStartMenu()
    {

    }
}
