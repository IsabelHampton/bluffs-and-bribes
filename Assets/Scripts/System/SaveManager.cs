using System;
using System.IO;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    private SaveData saveData = new SaveData();
    const string kJsonFileName = "playerData";

    private void Awake()
    {
        Instance = this;
    }

    private void OnApplicationPause(bool _pause)
    {
        if (_pause)
            Save();
    }
    private void OnApplicationQuit()
    {
        Save();
    }
    /*private void OnApplicationFocus()
    {
        LoadInSaveData();
    }*/
    public void UpdateSaveData()
    {

         // Game Manager
        saveData.state = (int)GameManager.Instance.State;

        saveData.currentRoundNumber = GameManager.Instance.RoundNumber;
        saveData.currentOpponentsEliminated = GameManager.Instance.OpponentsElimatedThisRun;
        saveData.currentMaxChips = GameManager.Instance.MaxChipsThisRun;

        saveData.maxRoundsComplete = GameManager.Instance.maxRoundsComplete;
        saveData.maxChipsAchieved = GameManager.Instance.maxChipsAchieved;
        saveData.maxOpponentsEliminated = GameManager.Instance.maxOpponentsEliminated;
        saveData.runsPlayed = GameManager.Instance.runsPlayed;

        // Action Manager

        saveData.hasFolded = ActionManager.Instance.hasFolded;
        saveData.isAllin = ActionManager.Instance.isAllIn;

        // Bribe Manager

        saveData.bribes = BribeManager.Instance.bribes;

        // Tip Off Manager

        saveData.unlockedTipOffs = TipOffManager.Instance.unlockedTipOffs.ToArray();

        // Bet Manager

        saveData.pot = BetManager.Instance.pot;
        saveData.chips = BetManager.Instance.chips;
        saveData.bets = BetManager.Instance.bets;

        // Hand Manager
        saveData.cardsOnTable = HandManager.Instance.cardsOnTable;
    }
    public void Save()
    {
        UpdateSaveData();
        string json = JsonUtility.ToJson(saveData);

        string path = GetDataPath();
        CreateFileIfNeeded(path);

        if(File.Exists(path)) // Just in case file creation fails
            File.WriteAllText(path, json);
    }
    public bool ReadSaveData()
    {
        string path = GetDataPath();
        if (!File.Exists(path))
            return false; // No data to read, exit out

        string readDataText = File.ReadAllText(path);
        saveData = JsonUtility.FromJson<SaveData>(readDataText);
        return true;
    }

    public void LoadInSaveData()
    {
        if (ReadSaveData())
            GameManager.Instance.ignoreResets = true;

        // Initialise all cards on table
        for (int i = 0; i < HandManager.Instance.cardsOnTable.Length; i++)
        {
            HandManager.Instance.cardsOnTable[i].Init(saveData.cardsOnTable[i]);
        }

        // Game Manager

        GameManager.Instance.RoundNumber = saveData.currentRoundNumber;
        GameManager.Instance.OpponentsElimatedThisRun = saveData.currentOpponentsEliminated;
        GameManager.Instance.MaxChipsThisRun = saveData.currentMaxChips;

        GameManager.Instance.maxRoundsComplete = saveData.maxRoundsComplete;
        GameManager.Instance.maxChipsAchieved = saveData.maxChipsAchieved;
        GameManager.Instance.maxOpponentsEliminated = saveData.maxOpponentsEliminated;
        GameManager.Instance.runsPlayed = saveData.runsPlayed;

        // Action Manager

         ActionManager.Instance.hasFolded = saveData.hasFolded;
         ActionManager.Instance.isAllIn = saveData.isAllin;

        // Bribe Manager

        BribeManager.Instance.bribes = saveData.bribes;

        // Tip Off Manager

        TipOffManager.Instance.unlockedTipOffs.Clear();
        foreach (GameObject tipOff in saveData.unlockedTipOffs)
        {
            TipOffManager.Instance.unlockedTipOffs.Add(tipOff);
        }

        // Bet Manager

        BetManager.Instance.SetPot(saveData.pot);
        BetManager.Instance.ReplaceChips(saveData.chips);
        BetManager.Instance.ReplaceBets(saveData.bets);

        // Put game on correct state
        GameManager.Instance.UpdateGameState((GameState)saveData.state);
    }

    public string GetDataPath()
    {
        if (Directory.Exists(Application.persistentDataPath))
        {
            return Path.Combine(Application.persistentDataPath, kJsonFileName);
        }
        return Path.Combine(Application.streamingAssetsPath, kJsonFileName);
    }

    public void CreateFileIfNeeded(string _path)
    {
        if (!File.Exists(_path))
            File.Create(_path).Close();
    }
    
}

[Serializable] 
public class SaveData
{
    // Game Manager
    public int state;

    public int currentRoundNumber;
    public int currentOpponentsEliminated;
    public int currentMaxChips;

    public int maxRoundsComplete;
    public int maxChipsAchieved;
    public int maxOpponentsEliminated;
    public int runsPlayed;

    // Action Manager

    public bool[] hasFolded;
    public bool[] isAllin;

    // Bribe Manager

    public int[] bribes;

    // Tip Off Manager

    public GameObject[] unlockedTipOffs;

    // Bet Manager

    public int pot;
    public int[] chips;
    public int[] bets;

    // Hand Manager
    public Card[] cardsOnTable;

}