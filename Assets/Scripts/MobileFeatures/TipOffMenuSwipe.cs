using UnityEngine;
using UnityEngine.UI;
public class TipOffMenuSwipe : MonoBehaviour
{
    // TO DO: Add check for interactive UI enabled
    private void Update()
    {
        // Only do check if tip off menu enabled
        if (!TipOffUI.Instance.tipOffMenu.activeInHierarchy)
            return;

        // Only check for left swipe if can move to next item
        if (TipOffUI.Instance.nextTipOffButton.interactable && MobileInputManager.Instance.CheckForSwipeLeft())
            TipOffUI.Instance.NextTipOff();
        // Only check for right swipe if can move to previous item
        else if (TipOffUI.Instance.previousTipOffButton.interactable && MobileInputManager.Instance.CheckForSwipeRight())
            TipOffUI.Instance.PreviousTipOff();
    }
}
