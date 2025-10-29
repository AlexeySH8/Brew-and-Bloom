using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class GameSceneManager : MonoBehaviour, IDataPersistence
{
    private const string HouseSceneName = "House";
    private const string TavernSceneName = "Tavern";

    public string SavedSceneName { get; private set; } = HouseSceneName;
    public event Action OnTavernLoaded;
    public event Action OnHouseLoaded;
    public event Action OnTavernUnloading;
    public event Action OnHouseUnloading;
    private IDataPersistenceManager _persistenceManager;

    public string CurrenSceneName => SceneManager.GetActiveScene().name;

    [Inject]
    public void Construct(IDataPersistenceManager dataPersistenceManager)
    {
        _persistenceManager = dataPersistenceManager;
        _persistenceManager.Register(this);
    }

    private void Awake()
    {
        SceneManager.sceneLoaded += HandleSceneLoaded;
    }

    private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        switch (scene.name)
        {
            case HouseSceneName:
                OnHouseLoaded?.Invoke();
                break;
            case TavernSceneName:
                OnTavernLoaded?.Invoke();
                break;
        }
    }

    public void LoadCurrentScene()
    {
        switch (_persistenceManager.GameData.SavedSceneName)
        {
            case HouseSceneName:
                LoadHouseScene();
                break;
            case TavernSceneName:
                LoadTavernScene();
                break;
        }
    }

    public void LoadHouseScene()
    {
        if (CurrenSceneName == TavernSceneName)
        {
            OnTavernUnloading?.Invoke();
            _persistenceManager.SaveGame();
        }
        SceneManager.LoadScene(HouseSceneName);
    }

    public void LoadTavernScene()
    {
        if (CurrenSceneName == HouseSceneName)
        {
            OnHouseUnloading?.Invoke();
            _persistenceManager.SaveGame();
        }
        SceneManager.LoadScene(TavernSceneName);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= HandleSceneLoaded;
    }

    public void LoadData(GameData gameData)
    {
        SavedSceneName = gameData.SavedSceneName;
    }

    public void SaveData(GameData gameData)
    {
        if (CurrenSceneName != "Menu")
            gameData.SavedSceneName = CurrenSceneName;
    }
}
