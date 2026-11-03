using UnityEngine;
using UnityEngine.InputSystem;

public class AccelerometerManager : MonoBehaviour
{
    public static AccelerometerManager Instance;
    private float shakingThreshold = 2.0f;

    private void Awake()
    {
        Instance = this;
    }
    public void EnableAccelerometer()
    {
        if (Accelerometer.current == null)
            return;

        InputSystem.EnableDevice(Accelerometer.current);
    }
    public void DisableAccelerometer()
    {
        if (Accelerometer.current == null)
            return;

        InputSystem.DisableDevice(Accelerometer.current);
    }
    public Vector3 GetAcceleration()
    {
        if (Accelerometer.current == null)
            return Vector3.zero;

        return Accelerometer.current.acceleration.ReadValue();
    }
    public bool IsShaking()
    {
        Debug.Log(GetAcceleration().magnitude);
        return GetAcceleration().magnitude > shakingThreshold;
    }
}
