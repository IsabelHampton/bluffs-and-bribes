using UnityEngine;
using TMPro;

public class TutorialUI : MonoBehaviour
{
    public static TutorialUI Instance;

    public GameObject TutorialPanel;
    public TMP_Text DescriptionDisplay;

    public HandTapArea[] handDisplays;

    void Awake()
    {
        Instance = this;
    }

    public void OpenMenu()
    {
        TutorialPanel.SetActive(true);
    }
    public void CloseMenu()
    {
        TutorialPanel.SetActive(false);
    }
    public void SelectHand(HandTapArea _handPanel)
    {
        Debug.Log("Hand Selected");
        if (_handPanel.selected)
            return; // Already selected so skip

        // De-select all other displays
        foreach (HandTapArea hand in handDisplays)
        { 
            DeselectHand(hand);
        }

        // Select this hand
        _handPanel.selectedHighlight.SetActive(true);
        _handPanel.selected = true;
        DescriptionDisplay.text = _handPanel.kDescription;
    }
    public void DeselectHand(HandTapArea _handPanel)
    {
        _handPanel.selectedHighlight.SetActive(false);
        _handPanel.selected = false;
    }
}
