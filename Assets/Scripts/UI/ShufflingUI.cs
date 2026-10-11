using UnityEngine;

public class ShufflingUI : MonoBehaviour
{
    // TO DO: Replace with 2D animation instead of different game objects - performance diff?
    public GameObject[] CardsToShuffle;
    public GameObject ShufflingPanel;
    public GameObject ShakeText;

    public float creationTime;
    float lifetime = 3f; // TO DO: chnage - this is for testign as currently not implemented

    public float shakeCheckCooldown = 0.2f;
    public float lastShakeCheck;

    public static ShufflingUI Instance;
    void Awake()
    {
        Instance = this;
        GameManager.OnGameStateChanged += GameManagerOnStateChanged;
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnStateChanged;
    }
    private void GameManagerOnStateChanged(GameState _state)
    {
        if (GameManager.Instance.State == GameState.DEALING && !ShufflingPanel.activeInHierarchy)
            ShowShufflingPanel();
        else if (GameManager.Instance.State != GameState.DEALING && ShufflingPanel.activeInHierarchy)
            HideShufflingPanel();
    }
    public void Update()
    {
        if (ShufflingPanel.activeInHierarchy)
        {
            Debug.Log($"Accelerator detect shaking: {AccelerometerManager.Instance.IsShaking()}");
            Debug.Log($"Gyroscope detect shaking: {GyroscopeManager.Instance.IsShaking()}");
            if (Time.time - creationTime > lifetime)
            {
                HideShufflingPanel();
                GameManager.Instance.UpdateGameState(GameState.PLAYER_TURN);
            }
            /*else
                PerformShakeAnimation();*/
        }
    }
    public void ShowShufflingPanel()
    {
        creationTime = Time.time;
        ShufflingPanel.SetActive(true);
        GyroscopeManager.Instance.EnableGyroSensor();
        AccelerometerManager.Instance.EnableAccelerometer();
    }
    public void HideShufflingPanel()
    {
        ShufflingPanel.SetActive(false);
        GyroscopeManager.Instance.DisableGyroSensor();
        AccelerometerManager.Instance.DisableAccelerometer();
    }

    public void PerformShakeAnimation()
    {
        int xVelocity = 0;
        int yVelocity = 0;

        // If need to do new shake check
        if (Time.time - lastShakeCheck > shakeCheckCooldown)
        {
            if (GyroscopeManager.Instance.IsShaking())
            {
                ShakeText.SetActive(true);
                xVelocity = Random.Range(-10, 10);
                yVelocity = Random.Range(-10, 10);
            }
            else
            {
                ShakeText.SetActive(false);
                xVelocity = 0;
                yVelocity = 0;
            }
        }
        foreach (GameObject card in CardsToShuffle)
        {
                card.transform.position = new Vector3(
                card.transform.position.x + xVelocity,
                card.transform.position.y + yVelocity,
                card.transform.position.z
                );
        }
    }
    


}
