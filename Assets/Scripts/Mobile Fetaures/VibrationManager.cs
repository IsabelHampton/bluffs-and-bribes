using UnityEngine;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void VibratePhone()
    {
        Handheld.Vibrate();
    }
}
