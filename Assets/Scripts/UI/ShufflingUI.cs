using UnityEngine;

public class ShufflingUI : MonoBehaviour
{
    // TO DO: Replace with 2D animation instead of different game objects - performance diff?
    public GameObject[] CardsToShuffle;
    public GameObject ShufflingPanel; 

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
        PerformShakeAnimation();
    }
    public void ShowShufflingPanel()
    {
        ShufflingPanel.SetActive(true);
    }
    public void HideShufflingPanel()
    {
        ShufflingPanel.SetActive(false);
    }

    public void PerformShakeAnimation()
    {
        if (GyroscopeManager.Instance.IsShaking())
        {
            foreach (GameObject card in CardsToShuffle)
            {
                card.transform.position = new Vector3(
                card.transform.position.x + Random.Range(5, 10),
                card.transform.position.y + Random.Range(5, 10),
                card.transform.position.z
                );
            }
        }
    }
    


}
