using System.Collections.Generic;
using UnityEngine;

public class TipOffManager : MonoBehaviour
{
    public static TipOffManager Instance;
    public List<GameObject> unlockedTipOffs = new List<GameObject>();
    [SerializeField] GameObject[] tipoffs = new GameObject[10];

    HashSet<int> roundsWithTipOffs = new HashSet<int> {0, 1, 2, 3, 5 };

    private void Awake()
    {
        Instance = this;
        GameManager.OnGameStateChanged += GameManagerOnStateChanged;

        // REMOVE
        unlockedTipOffs.Add(tipoffs[0]);
        unlockedTipOffs.Add(tipoffs[1]);
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnStateChanged;
    }

    private void GameManagerOnStateChanged(GameState _state)
    {
        if (GameManager.Instance.State == GameState.TIPOFF)
        {
            if (TutorialManager.Instance.TutorialSkipsTipOff())
            {
                GameManager.Instance.UpdateGameState(GameState.BRIBE);
                return; // Skip rest of method
            }

            // TO DO: Implement tipoffs

            GameManager.Instance.UpdateGameState(GameState.BRIBE);
        }
    }
    public void ReceiveTipOff(int _roundNumber)
    {
        if (roundsWithTipOffs.Contains(_roundNumber))
        {
            unlockedTipOffs.Add(tipoffs[unlockedTipOffs.Count]); //adds next tipoff to unocked tipoffs list
        }
    }

}
