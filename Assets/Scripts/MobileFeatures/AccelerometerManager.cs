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
        InputSystem.EnableDevice(Accelerometer.current);
    }
    public void DisableAccelerometer()
    {
        InputSystem.DisableDevice(Accelerometer.current);
    }
    public Vector3 GetAcceleration()
    {
        return Accelerometer.current.acceleration.ReadValue();
    }
    public bool IsShaking()
    {
        Debug.Log(GetAcceleration().magnitude);
        return GetAcceleration().magnitude > shakingThreshold;
    }
}
