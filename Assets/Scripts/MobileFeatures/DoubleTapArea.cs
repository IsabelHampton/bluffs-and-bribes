using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
public class DoubleTapArea : MonoBehaviour
{
    public float lastValidTouchTime;
    public GameObject checkArea;
    Vector2 checkAreaScreenPos;
    float checkAreaWidth;
    float checkAreaHeight;

    private void Awake()
    {
        GameManager.OnGameStateChanged += GameManagerOnStateChanged;

        RectTransform rectTransform = checkArea.GetComponent<RectTransform>();
        checkAreaScreenPos = rectTransform.position; // top right corner
        checkAreaWidth = rectTransform.rect.width;
        checkAreaHeight = rectTransform.rect.height;

        checkArea.SetActive(false); // Should be hidden at start
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnStateChanged;
    }

    private void GameManagerOnStateChanged(GameState _state)
    { 
        // Only show area when player can check
        if (GameManager.Instance.State == GameState.PLAYER_TURN && ActionManager.Instance.CanCheck((int)PlayerOrder.PLAYER)) // TO DO: Add check that interctaive UI selected
        {
            checkArea.SetActive(true);
        }
    }
    private void Update()
    {
        // Only check if area active
        if (!checkArea.activeInHierarchy)
            return;

        // Iterate through all current detected touches
        foreach (Touch touch in Touch.activeTouches)
        {
            Debug.Log($"Touch location: {touch.screenPosition}, check area pos: {checkAreaScreenPos}, width: {checkAreaWidth}, height {checkAreaHeight}");
            // Check if this touch is a tap and if the tap count > 1 and is within bounds of check area

            if (touch.isTap && touch.tapCount > 1 && TouchIsInCheckArea(touch))
            {
                    checkArea.SetActive(false);
                    ActionManager.Instance.PlayerCheck();
                    Debug.Log("Check complete by double tap");  
            }
        }

    }
    public bool TouchIsInCheckArea(Touch _touch)
    {
        return _touch.screenPosition.x > checkAreaScreenPos.x - checkAreaWidth &&
                _touch.screenPosition.x < checkAreaScreenPos.x &&
                _touch.screenPosition.y > checkAreaScreenPos.y - checkAreaHeight &&
                _touch.screenPosition.y < checkAreaScreenPos.y;
    }
}
