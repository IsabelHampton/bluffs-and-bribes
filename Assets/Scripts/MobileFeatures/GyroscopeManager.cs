using UnityEngine;

public class GyroscopeManager : MonoBehaviour
{
    public static GyroscopeManager Instance;
    public float shakeSpeed = 100.0f;
    private void Awake()
    {
        Instance = this;
        DisableGyroSensor(); // Should be off by default - saves battery
    }
    public bool IsShaking()
    {
        bool isShaking = false;
        //Input.gyro.enabled = true; // Turn on gyro sensor
        Debug.Log($" rotation rate magnitude = {Input.gyro.rotationRate.magnitude}");

        if (Input.gyro.rotationRate.magnitude > shakeSpeed)
            isShaking = true;

        Debug.Log($"Checking for shaking, result: {isShaking}");
        return isShaking;
    }

    public void EnableGyroSensor()
    {
        if(SystemInfo.supportsGyroscope)
            Input.gyro.enabled = true;
    }
    public void DisableGyroSensor()
    {
        if (SystemInfo.supportsGyroscope)
            Input.gyro.enabled = false;
    }
}
