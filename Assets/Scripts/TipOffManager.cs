using UnityEngine;

public class TipOffManager : MonoBehaviour
{
    //[SerializeField] int tipOffNumber;
    int unlockedTipOffs;
    

    private void Awake()
    {
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
    public void ReceiveTipOff(int _tipOffNumber)
    {
        Debug.Log("Welcome to Game");
    }

}

public enum TipoffTiming
{
    TIPOFF1 = 0,
    TIPOFF2 = 1,
    TIPOFF3 = 3,
    TIPOFF4 = 5
}