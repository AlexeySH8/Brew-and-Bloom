using System.Collections.Generic;
using UnityEngine;

public class DataPersistenceManager : IDataPersistenceManager
{
    public GameData GameData { get; private set; }
    private List<IDataPersistence> _dataPersistences = new();
    private const string FileName = "save.json";
    private FileDataHandler _dataHandler;

    public DataPersistenceManager()
    {
        string dataPath = Application.persistentDataPath;
        _dataHandler = new FileDataHandler(dataPath, FileName);
    }

    public void NewGame()
    {
        GameData = new GameData();
        SaveGame();
    }

    public void SaveGame()
    {
        if (GameData == null)
        {
            Debug.Log("Cannot save: GameData is null.");
            return;
        }

        foreach (IDataPersistence persistence in _dataPersistences)
            persistence.SaveData(GameData);

        _dataHandler.SaveData(GameData);
    }

    public void LoadGame()
    {
        GameData = _dataHandler.LoadData();

        if (GameData == null)
        {
            Debug.Log("No save file found. Creating new game.");
            NewGame();
            return;
        }

        foreach (IDataPersistence persistence in _dataPersistences)
            persistence.LoadData(GameData);
    }

    public void Register(IDataPersistence persistence)
    {
        if (!_dataPersistences.Contains(persistence))
            _dataPersistences.Add(persistence);
    }
}
