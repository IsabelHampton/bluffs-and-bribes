using UnityEngine;
using UnityEngine.UI;

public class BribeManager : MonoBehaviour
{
    public BribeManager Instance;
    public Button bribeButton;

    public void UpdateInteractability(bool _isInteractable)
    {
        bribeButton.interactable = _isInteractable;
    }

    private void Awake()
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
        UpdateInteractability(GameManager.Instance.state == GameState.BRIBE);

        if (GameManager.Instance.state == GameState.BRIBE)
        {
        }
        else
        {

        }
    }


}
