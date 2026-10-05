using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class MobileInputManager : MonoBehaviour
{
    public static MobileInputManager Instance;

    private float swipeThreshold = 50; // measured in px?

    private void Awake()
    {
        Instance = this;
    }
    private void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }
    private void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }
    public Vector2 CheckForSwipeDirection()
    {
        Vector2 swipeDirection = new Vector2(0, 0);
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
            Vector2 touchVector = touch.screenPosition - touch.startScreenPosition;

            if (touchVector.magnitude > swipeThreshold)
                swipeDirection = touchVector.normalized; // Swipe found, return out of method
        }
        return swipeDirection; // Will return (0, 0) if no swipe found
    }

    public bool CheckForSwipeLeft()
    {
        return CheckForSwipeDirection().x < 0;
    }
    public bool CheckForSwipeRight()
    {
        return CheckForSwipeDirection().x > 0;
    }
    public bool CheckForSwipeUp()
    {
        return CheckForSwipeDirection().y < 0;
    }
    public bool CheckForSwipeDown()
    {
        return CheckForSwipeDirection().x > 0;
    }

}
