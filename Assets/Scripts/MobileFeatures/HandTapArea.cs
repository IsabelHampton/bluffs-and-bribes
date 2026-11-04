using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
public class HandTapArea : MonoBehaviour
{
    public GameObject selectedHighlight;
    public string kDescription;
    public string kHandName;

    public bool selected;

    public void OnSelected()
    {
        TutorialUI.Instance.SelectHand(this);
        VibrationManager.Instance.MediumVibrate();
    }
}
