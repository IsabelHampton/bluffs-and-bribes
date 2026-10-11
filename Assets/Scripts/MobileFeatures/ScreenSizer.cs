using UnityEngine;

public class ScreenSizer : MonoBehaviour
{
    [SerializeField] RectTransform[] fullScreenPanels;
    Rect lastSafe;
    Vector2Int lastDimensions;

    private void Update()
    {
        Vector2Int currentDimenisions= new Vector2Int(Screen.width, Screen.height);
        // If screen hasn't changed no need to resize
        if (Screen.safeArea == lastSafe && lastDimensions == currentDimenisions)
            return;
        // Screeen size has changed so update properties
        lastSafe = Screen.safeArea;
        lastDimensions = currentDimenisions;

        // Update anchors
        Vector2 min = lastSafe.min;
        Vector2 max = lastSafe.max;
        foreach (RectTransform transform in fullScreenPanels)
        {
            transform.anchorMin = min;
            transform.anchorMax = max;
            transform.offsetMin = Vector2.zero;
            transform.offsetMax = Vector2.one;
        }
    }
}
