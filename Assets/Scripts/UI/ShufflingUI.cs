using UnityEngine;

public class ShufflingUI : MonoBehaviour
{
    // TO DO: Replace with 2D animation instead of different game objects - performance diff?
    public GameObject[] CardsToShuffle;
    public GameObject ShufflingPanel;
    public float creationTime;
    float lifetime = 3f; // TO DO: chage - this is for testign as currently not implemented

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
            if (Time.time - creationTime > lifetime)
            {
                HideShufflingPanel();
                GameManager.Instance.UpdateGameState(GameState.PLAYER_TURN);
            }
            else
                PerformShakeAnimation();
        }
    }
    public void ShowShufflingPanel()
    {
        creationTime = Time.time;
        ShufflingPanel.SetActive(true);
        GyroscopeManager.Instance.EnableGyroSensor();
    }
    public void HideShufflingPanel()
    {
        ShufflingPanel.SetActive(false);
        GyroscopeManager.Instance.DisableGyroSensor();
    }

    public void PerformShakeAnimation()
    {
        if (GyroscopeManager.Instance.IsShaking())
        {
            foreach (GameObject card in CardsToShuffle)
            {
                card.transform.position = new Vector3(
                card.transform.position.x + Random.Range(-10, 10),
                card.transform.position.y + Random.Range(-10, 10),
                card.transform.position.z
                );
            }
        }
    }
    


}
