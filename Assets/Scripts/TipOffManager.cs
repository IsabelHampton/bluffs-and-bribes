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
    }
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= GameManagerOnStateChanged;
    }

    private void GameManagerOnStateChanged(GameState _state)
    {
        if (_state == GameState.TIPOFF)
        {
            ReceiveTipOff(GameManager.Instance.roundNumber);
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
