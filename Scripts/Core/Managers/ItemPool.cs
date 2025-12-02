using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class ItemPool : MonoBehaviour, IDataPersistence
{
    public static ItemPool Instance { get; private set; }

    private const int MaxItemCountInScene = 150;
    private IDataPersistenceManager _dataPersistenceManager;
    private GameSceneManager _gameSceneManager;
    private List<BaseHoldItem> _registeredHoldItems = new();
    private List<SceneItemData> _scenesItemsSaveData = new();

    [Inject]
    public void Construct(IDataPersistenceManager dataPersistenceManager, GameSceneManager gameSceneManager)
    {
        _gameSceneManager = gameSceneManager;
        _dataPersistenceManager = dataPersistenceManager;
        _dataPersistenceManager.Register(this);
        _gameSceneManager.OnTavernUnloading += UnregisterTavernItems; 
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogWarning("Duplicate ItemPool detected, destroying new one.");
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Register(BaseHoldItem newItem)
    {
        if (_registeredHoldItems.Contains(newItem)) return;

        if (_registeredHoldItems.Count >= MaxItemCountInScene)
            DiscardOldestItem();

        _registeredHoldItems.Add(newItem);
    }

    public void Unregister(BaseHoldItem item)
    {
        _registeredHoldItems.Remove(item);
    }

    private void DiscardOldestItem()
    {
        BaseHoldItem oldestItem = _registeredHoldItems.FirstOrDefault();
        oldestItem.Discard();
    }

    private void UnregisterTavernItems()
    {
        var registeredHoldItems = new List<BaseHoldItem>(_registeredHoldItems);
        foreach (var item in registeredHoldItems)
            Unregister(item);
    }

    public void LoadData(GameData gameData)
    {
        _scenesItemsSaveData = gameData.ScenesItemsData;
        SpawnSavedItems();
    }

    public void SaveData(GameData gameData)
    {
        string currentScene = SceneManager.GetActiveScene().name;
        List<ItemSaveData> itemsToSave = new List<ItemSaveData>();

        foreach (var item in _registeredHoldItems)
        {
            ItemSaveData itemSave = new ItemSaveData();

            itemSave.PrefabPath = item.PrefabPath;
            itemSave.Position[0] = item.transform.position.x;
            itemSave.Position[1] = item.transform.position.y;
            itemSave.Position[2] = item.transform.position.z;

            itemSave.Rotation[0] = item.transform.rotation.x;
            itemSave.Rotation[1] = item.transform.rotation.y;
            itemSave.Rotation[2] = item.transform.rotation.z;
            itemSave.Rotation[3] = item.transform.rotation.w;

            itemsToSave.Add(itemSave);
        }

        SceneItemData currentSceneData = gameData.ScenesItemsData
            .FirstOrDefault(s => s.SceneName == currentScene);

        if (currentSceneData == null)
        {
            currentSceneData = new SceneItemData() { SceneName = currentScene };
            gameData.ScenesItemsData.Add(currentSceneData);
        }
        currentSceneData.ItemsSaveData = itemsToSave;
    }

    private void SpawnSavedItems()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        var itemsSaveData = _scenesItemsSaveData
            .FirstOrDefault(s => s.SceneName == currentScene);

        if (itemsSaveData == null) return;

        foreach (var itemSave in itemsSaveData.ItemsSaveData)
        {
            GameObject prefab = Resources.Load<GameObject>(itemSave.PrefabPath);
            Vector3 position = new Vector3(
                itemSave.Position[0], itemSave.Position[1], itemSave.Position[2]);
            Quaternion rotation = new Quaternion(
                itemSave.Rotation[0], itemSave.Rotation[1],
                itemSave.Rotation[2], itemSave.Rotation[3]);

            GameObject itemObj = Instantiate(prefab, position, rotation);
        }
    }

    private void OnDisable()
    {
        _gameSceneManager.OnTavernUnloading -= UnregisterTavernItems;
        _dataPersistenceManager.Unregister(this);
    }
}
