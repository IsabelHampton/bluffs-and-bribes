using UnityEngine;
using UnityEngine.InputSystem;
using Gyroscope = UnityEngine.InputSystem.Gyroscope;

public class GyroscopeManager : MonoBehaviour
{
    public static GyroscopeManager Instance;
    private float shakeSpeed = 10.0f;
    private void Awake()
    {
        Instance = this;
        DisableGyroSensor(); // Should be off by default - saves battery
    }
    public bool IsShaking()
    {
        // If gyro sensor off, enable before check

        if(!Gyroscope.current.enabled)
            EnableGyroSensor();

        bool isShaking = false;
        Debug.Log($"Gyro present: {Gyroscope.current}");
        Debug.Log($"Gyro velovity: {Gyroscope.current?.angularVelocity.ReadValue()}");
        Debug.Log($"Gyro speed: {Gyroscope.current?.angularVelocity.ReadValue().magnitude}");

        if (Gyroscope.current?.angularVelocity.ReadValue().magnitude > shakeSpeed)
            isShaking = true;

        Debug.Log($"Checking for shaking, result: {isShaking}");
        return isShaking;
    }

    public void EnableGyroSensor()
    {
        if (Gyroscope.current != null)
        {
            InputSystem.EnableDevice(Gyroscope.current);
        }
    }
    public void DisableGyroSensor()
    {
        if (Gyroscope.current != null)
        {
            InputSystem.DisableDevice(Gyroscope.current);
        }
    }
}
