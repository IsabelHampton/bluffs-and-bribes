using UnityEngine;
using UnityEngine.UI;
public class TipOffUI : MonoBehaviour
{
    public static TipOffUI Instance;

    [SerializeField] public GameObject tipOffMenu;
    [SerializeField] public Button nextTipOffButton;
    [SerializeField] public Button previousTipOffButton;
    [SerializeField] private Button tipOffButton;

    private int viewingTipOff;

    private void Awake()
    {
        Instance = this;
    }
    public void OpenTipoffMenu()
    {
        tipOffMenu.SetActive(true);
        viewingTipOff = 0;
        TipOffManager.Instance.unlockedTipOffs[0].SetActive(true);
        previousTipOffButton.interactable = false;

        if (TipOffManager.Instance.unlockedTipOffs.Count > 1)
            nextTipOffButton.interactable = true;
        else
            nextTipOffButton.interactable = false;
    }
    public void HideTipOffMenu()
    {
        tipOffMenu.SetActive(false);
    }
    public void NextTipOff()
    {
        previousTipOffButton.interactable = true;

        TipOffManager.Instance.unlockedTipOffs[viewingTipOff].SetActive(false);
        viewingTipOff++;
        TipOffManager.Instance.unlockedTipOffs[viewingTipOff].SetActive(true);

        if (viewingTipOff == TipOffManager.Instance.unlockedTipOffs.Count - 1)
            nextTipOffButton.interactable = false;
    }
    public void PreviousTipOff()
    {
        nextTipOffButton.interactable = true;

        TipOffManager.Instance.unlockedTipOffs[viewingTipOff].SetActive(false);
        viewingTipOff--;
        TipOffManager.Instance.unlockedTipOffs[viewingTipOff].SetActive(true);

        if (viewingTipOff == 0)
            previousTipOffButton.interactable = false;
    }
}
