using UnityEngine;

public class GyroscopeManager : MonoBehaviour
{
    public float shakeSpeed;
    private void Awake()
    {
        Input.gyro.enabled = false; // Should be off by default - saves battery
    }
    public bool IsShaking()
    {
        bool isShaking = false;
        Input.gyro.enabled = true; // Turn on gyro sensor

        if (Input.gyro.rotationRateUnbiased.magnitude > shakeSpeed)
            isShaking = true;

        Input.gyro.enabled = false; // Chekc complete turn off sensor
        return isShaking;
    }
}
