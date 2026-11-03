using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
public class HandTapArea : MonoBehaviour
{

    public GameObject tapArea;
    public GameObject selectedHighlight;
    public string description;

    Vector2 tapAreaScreenPos;
    float tapAreaWidth;
    float tapAreaHeight;

    public bool selected;

    private void Awake()
    {

        RectTransform rectTransform = tapArea.GetComponent<RectTransform>();
        tapAreaScreenPos = rectTransform.position; // top right corner
        tapAreaWidth = rectTransform.rect.width;
        tapAreaHeight = rectTransform.rect.height;
    }
    private void OnEnable()
    {
        // One will be selected by default
        if (selected)
            TutorialUI.Instance.SelectHand(this);
    }
    private void Update()
    {
        // Only check if area active
        if (!tapArea.activeInHierarchy)
            return;

        // Iterate through all current detected touches
        foreach (Touch touch in Touch.activeTouches)
        {
            Debug.Log($"Touch location: {touch.screenPosition}, check area pos: {tapAreaScreenPos}, width: {tapAreaWidth}, height {tapAreaHeight}");
            // Check if this touch is a tap and if the tap count > 1 and is within bounds of check area

            if (touch.isTap && TouchIsInCheckArea(touch))
            {
                TutorialUI.Instance.DescriptionDisplay.text = description;
                Debug.Log("Hand selected from menu");
            }
        }

    }
    public bool TouchIsInCheckArea(Touch _touch)
    {
        return _touch.screenPosition.x > tapAreaScreenPos.x - tapAreaWidth &&
                _touch.screenPosition.x < tapAreaScreenPos.x &&
                _touch.screenPosition.y > tapAreaScreenPos.y - tapAreaHeight &&
                _touch.screenPosition.y < tapAreaScreenPos.y;
    }
}
