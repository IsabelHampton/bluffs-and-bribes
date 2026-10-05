using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class MobileInputManager : MonoBehaviour
{
    private float swipeThreshold = 50; // measured in px?
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }
    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }
    public bool CheckIfSwiped()
    {
        // Iterate through all active touches
        foreach (Touch touch in Touch.activeTouches)
        {
            // Skip if touch is a tap
            if (touch.isTap)
                continue;

            // Skip if touch is on going - should only swipe on release
            if (touch.isInProgress)
                continue;

            // Check if start pos and end pos distance are enough to be a swipe
            float touchDistance = (touch.screenPosition - touch.startScreenPosition).magnitude;

            if (touchDistance > swipeThreshold)
                return true; // Swipe found, return out of method
        }
        return false;
    }
}
