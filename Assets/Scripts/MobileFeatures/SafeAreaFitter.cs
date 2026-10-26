// CMP5402 Mobile Development - Week 3: Responsive UI & Mobile UX
// SafeAreaFitter: the "SafeAreaFitter script" slide.
// Resizes this RectTransform to match Screen.safeArea, so buttons and text stay clear of notches,
// camera holes, rounded corners and the home indicator.
//
// Set-up:
//   Canvas
//     Background        <- outside the safe area, so it can bleed to the edges
//     SafeArea          <- stretch to fill the Canvas, then add this script
//       HUD, Controls, PauseMenu ...
//
// The safe area changes on rotation, when a foldable opens and in split-screen, so it is checked every
// frame. Comparing two values per frame is cheap; the anchors are only rewritten when something changes.
// Screen.safeArea is in pixels with its origin at the bottom-left, which matches uGUI anchor space.

using UnityEngine;
[RequireComponent(typeof(RectTransform))]
public class SafeAreaFitter : MonoBehaviour
{
    [Tooltip("Turn an edge off to let that side ignore the safe area, for example a bottom bar that should sit under the home indicator.")]
    [SerializeField] bool applyLeft = true, applyRight = true, applyTop = true, applyBottom = true;

    RectTransform panel;
    Rect lastSafe;
    Vector2Int lastSize;

    void Awake() => panel = GetComponent<RectTransform>();

    void OnEnable() => lastSize = Vector2Int.zero; // force a refresh

    void Update()
    {
        var size = new Vector2Int(Screen.width, Screen.height);
        Rect safe = Screen.safeArea;
        if (safe == lastSafe && size == lastSize) return;
        if (size.x <= 0 || size.y <= 0) return;

        lastSafe = safe;
        lastSize = size;

        // Convert pixel rectangle to 0-1 anchors.
        Vector2 min = safe.min, max = safe.max;
        min.x = applyLeft ? min.x / size.x : 0f;
        min.y = applyBottom ? min.y / size.y : 0f;
        max.x = applyRight ? max.x / size.x : 1f;
        max.y = applyTop ? max.y / size.y : 1f;

        panel.anchorMin = min;
        panel.anchorMax = max;
        panel.offsetMin = Vector2.zero;
        panel.offsetMax = Vector2.zero;
    }
}

