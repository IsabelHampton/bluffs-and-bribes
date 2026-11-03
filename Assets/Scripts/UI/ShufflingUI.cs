using UnityEngine;

public class ShufflingUI : MonoBehaviour
{
    // TO DO: Replace with 2D animation instead of different game objects - performance diff?
   // public GameObject[] CardsToShuffle;
    public GameObject ShufflingPanel;
    public GameObject ShakeText;

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

            if (!SettingsManager.Instance.LimitedMotionEnabled && !SettingsManager.Instance.PowerSavingEnabled) // Check if should have shake detection
                PerformShakeAnimation();
            //else
                // TO DO: Animate without motion
        }
    }
    public void MoveOnFromShuffling()
    {
        HideShufflingPanel();
        GameManager.Instance.UpdateGameState(GameState.PLAYER_TURN);
    }
    public void ShowShufflingPanel()
    {
        ShufflingPanel.SetActive(true);

        if (!SettingsManager.Instance.LimitedMotionEnabled && !SettingsManager.Instance.PowerSavingEnabled) // Check if should have shake detection
            AccelerometerManager.Instance.EnableAccelerometer();
    }
    public void HideShufflingPanel()
    {
        ShufflingPanel.SetActive(false);

        AccelerometerManager.Instance.DisableAccelerometer(); // Do always just in case
    }

    public void PerformShakeAnimation()
    {
        // If need to do new shake check
        if (Time.time - lastShakeCheck > shakeCheckCooldown)
        {
            lastShakeCheck = Time.time;
            if (AccelerometerManager.Instance.IsShaking())
            {
                ShakeText.SetActive(true);
            }
            else
            {
                ShakeText.SetActive(false);
            }
        }
        // TO DO: Implement animation
    }
    


}
