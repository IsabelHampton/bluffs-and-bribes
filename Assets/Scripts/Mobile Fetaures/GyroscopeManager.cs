using UnityEngine;

public class GyroscopeManager : MonoBehaviour
{
    public static GyroscopeManager Instance;
    public float shakeSpeed = 0;
    private void Awake()
    {
        Instance = this;
        Input.gyro.enabled = true; // Should be off by default - saves battery
    }
    public bool IsShaking()
    {
        bool isShaking = false;
        //Input.gyro.enabled = true; // Turn on gyro sensor
        Debug.Log($" Gyro enabled: {Input.gyro.enabled}");
        Debug.Log($" rotation rate magnitude = {Input.gyro.rotationRate.magnitude}");
        Debug.Log($" rotation rate x = {Input.gyro.rotationRate.x}");
        Debug.Log($" rotation rate y = {Input.gyro.rotationRate.y}");
        Debug.Log($" rotation rate z = {Input.gyro.rotationRate.z}");
        if (Input.gyro.rotationRate.magnitude > shakeSpeed)
            isShaking = true;

        //Input.gyro.enabled = false; // Chekc complete turn off sensor
        Debug.Log($"Checking for shaking, result: {isShaking}");
        return isShaking;
    }
}
