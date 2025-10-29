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
    private List<BaseHoldItem> _registeredHoldItems = new();
    private List<ItemSaveData> _itemsSaveData = new();

    [Inject]
    public void Construct(IDataPersistenceManager dataPersistenceManager)
    {
        _dataPersistenceManager = dataPersistenceManager;
        _dataPersistenceManager.Register(this);
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

    public void LoadData(GameData gameData)
    {
        string currentScene = SceneManager.GetActiveScene().name;
        SceneItemData sceneData = gameData.ScenesItems
            .FirstOrDefault(s => s.SceneName == currentScene);

        if (sceneData == null) return;

        _itemsSaveData = sceneData.ItemsSaveData;
        foreach (var itemSave in _itemsSaveData)
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

    public void SaveData(GameData gameData)
    {
        _itemsSaveData = new();
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

            _itemsSaveData.Add(itemSave);
        }

        string currentScene = SceneManager.GetActiveScene().name;
        SceneItemData sceneData = gameData.ScenesItems
            .FirstOrDefault(s => s.SceneName == currentScene);

        if (sceneData == null)
        {
            sceneData = new SceneItemData() { SceneName = currentScene };
            gameData.ScenesItems.Add(sceneData);
        }
        sceneData.ItemsSaveData = _itemsSaveData;
    }
}
