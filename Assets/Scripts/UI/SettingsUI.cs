using UnityEngine;
using UnityEngine.UI;
public class SettingsUI : MonoBehaviour
{
    public static SettingsUI Instance;

    public GameObject setttingsPanel;

    public void OpenSettings()
    {
        setttingsPanel.SetActive(true);
    }
    public void CloseSettings() 
    {
        setttingsPanel.SetActive(false);
    }

}
