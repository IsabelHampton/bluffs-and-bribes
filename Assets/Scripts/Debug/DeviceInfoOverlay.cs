using UnityEngine;

public class DeviceInfoOverlay : MonoBehaviour
{
    float smoothedDelta = 1f / 30f;
    GUIStyle style;
    private void Awake()
    {
        Application.targetFrameRate = 30;
    }
    void Update()
    {
        // TODO: blend Time.unscaledDeltaTime into smoothedDelta
        // so that the FPS figure does not flicker every frame.
        smoothedDelta += (Time.unscaledDeltaTime - smoothedDelta) * 0.1f;

    }
    void OnGUI()
    {
        if (style == null)
        {
            style = new GUIStyle(GUI.skin.box);
            style.alignment = TextAnchor.UpperLeft;
            style.normal.textColor = Color.white;
        }
        // TODO: a fixed font size is unreadable on a high-DPI phone.
        style.fontSize = Mathf.Max(14, Screen.height / 40);


        // Set style.fontSize relative to Screen.height instead.
        float fps = 1f / smoothedDelta;
        string info = $"FPS: {fps:0}\n" +
            $"Device model: {SystemInfo.deviceModel}\n" +
            $"OS: {SystemInfo.operatingSystem}\n" +
            $"Resolution: {Screen.currentResolution}\n" +
            $"DPI: {Screen.dpi:0}\n" +
            $"GPU: {SystemInfo.graphicsDeviceName}\n" +
            $"Graphics Memory: {SystemInfo.graphicsMemorySize}MB\n" +
            $"System memory: {SystemInfo.systemMemorySize}MB\n";
        // TODO: append device model, operating system, resolution,
        // refresh rate, DPI, GPU name, graphics memory and system memory.
        Vector2 size = style.CalcSize(new GUIContent(info));
        GUI.Label(new Rect(40, 40, size.x + 20, size.y + 20), info, style);
    }

}
