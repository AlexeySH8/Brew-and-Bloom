using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DataPersistenceManager : MonoBehaviour, IDataPersistenceManager
{
    public GameData GameData { get; private set; }
    private List<IDataPersistence> _dataPersistences = new();
    private const string FileName = "save.json";
    private FileDataHandler _dataHandler;
    private GameSceneManager _gameSceneManager;

    [Inject]
    public void Construct(GameSceneManager gameSceneManager)
    {
        string dataPath = Application.persistentDataPath;
        _dataHandler = new FileDataHandler(dataPath, FileName);
        _gameSceneManager = gameSceneManager;
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
            Debug.Log("No save file found.");
            return;
        }

        foreach (IDataPersistence persistence in _dataPersistences)
            persistence.LoadData(GameData);
    }

    public void LoadGameMeta()
    {
        GameData = _dataHandler.LoadData();
    }

    public void Register(IDataPersistence persistence)
    {
        if (!_dataPersistences.Contains(persistence))
            _dataPersistences.Add(persistence);
    }

    public void Unregister(IDataPersistence persistence)
    {
        _dataPersistences.Remove(persistence);
    }

    public bool HasSave() => _dataHandler.Exists();
}
