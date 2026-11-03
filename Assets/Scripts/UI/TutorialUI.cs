using UnityEngine;

public class TutorialUI : MonoBehaviour
{
    public static TutorialUI Instance;

    public GameObject tutorialPanel;

    void Awake()
    {
        Instance = this;
    }

    public void OpenMenu()
    {
        tutorialPanel.SetActive(true);
    }
    public void CloseMenu()
    {
        tutorialPanel.SetActive(false);
    }
}
