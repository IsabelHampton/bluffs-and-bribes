using UnityEngine;
using CandyCoded.HapticFeedback;

public class VibrationManager : MonoBehaviour
{
    public static VibrationManager Instance;
    private void Awake()
    {
        Instance = this;
    }
    public void LightVibrate()
    {
        HapticFeedback.LightFeedback();
    }
    public void MediumVibrate()
    {
        HapticFeedback.MediumFeedback();
    }
    public void HeavyVibrate()
    {
        HapticFeedback.HeavyFeedback();
    }
}
