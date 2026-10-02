using System;
using UnityEngine;
using TMPro;

public class ActionUI : MonoBehaviour
{
    public static ActionUI Instance;
    private string[] actionNames = { "Check", "Call", "Raise", "Fold", "All In" }; // TO DO: Make not dependent on order of Actions enum

    /*public TMP_Text playerActionDisplay;
    public TMP_Text e1ActionDisplay;
    public TMP_Text e2ActionDisplay;
    public TMP_Text e3ActionDisplay;*/

    public ActionDisplay[] displays = new ActionDisplay[(int)PlayerOrder.NUM_OF_PLAYERS];
    void Awake()
    {
        Instance = this;
    }

/*    public void UpdateElements()
    {
        playerActionDisplay.text = Convert.ToString(ActionManager.Instance.lastActions[(int)PlayerOrder.PLAYER]);
        e1ActionDisplay.text = Convert.ToString(ActionManager.Instance.lastActions[(int)PlayerOrder.OPPONENT1]);
        e2ActionDisplay.text = Convert.ToString(ActionManager.Instance.lastActions[(int)PlayerOrder.OPPONENT2]);
        e3ActionDisplay.text = Convert.ToString(ActionManager.Instance.lastActions[(int)PlayerOrder.OPPONENT3]);
    }*/

    public void DisplayActionUI(int _playerNumber, Actions _action)
    {
        // Prevent being done multipe times
        if (!displays[_playerNumber].gameObject.activeInHierarchy)
        {
            displays[_playerNumber].GetComponent<TMP_Text>().text = actionNames[(int)_action]; // Update what text will display
            displays[_playerNumber].gameObject.SetActive(true); // Enable text display
        }
    }
}
