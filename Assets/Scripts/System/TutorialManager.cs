using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       if (SaveManager.Instance.ReadSaveData())
        {
            if (SaveManager.Instance.saveData.runsPlayed == 0)
                PlayTutorial();
        }
       // No save file
        else
        {
            Debug.Log("No save file detected");
            PlayTutorial();
        }
    }
    void PlayTutorial()
    {

    }

}
