using System.Collections.Generic;
using UnityEngine;

public class DataPresistenceManager : IDataPersistenceManager
{
    public GameData GameData { get; private set; }
    private List<IDataPersistence> _dataPresistances = new();
    private const string FileName = "save.json";
    private FileDataHandler _dataHandler;

    public void NewGame()
    {
        GameData = new GameData();
        SaveGame();
    }

    public void SaveGame()
    {
        string dataPath = Application.persistentDataPath;
        _dataHandler = new FileDataHandler(dataPath, FileName);

        if (_dataHandler == null || GameData == null)
        {
            Debug.Log("No save file found. Creating new game.");
            NewGame();
            return;
        }

        foreach (IDataPersistence presistance in _dataPresistances)
            presistance.SaveData(GameData);

        _dataHandler.SaveData(GameData);
    }

    public void LoadGame()
    {
        string dataPath = Application.persistentDataPath;
        _dataHandler = new FileDataHandler(dataPath, FileName);

        GameData = _dataHandler?.LoadData();

        if (GameData == null)
        {
            Debug.Log("No save file found. Creating new game.");
            NewGame();
            return;
        }

        foreach (IDataPersistence presistance in _dataPresistances)
            presistance.LoadData(GameData);
    }

    public void Register(IDataPersistence presistance)
    {
        if (!_dataPresistances.Contains(presistance))
            _dataPresistances.Add(presistance);
    }
}
