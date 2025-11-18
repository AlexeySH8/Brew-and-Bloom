using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class ItemStandManager : MonoBehaviour, IDataPersistence
{
    private IDataPersistenceManager _persistenceManager;
    private List<ItemStand> _itemStands = new();

    [Inject]
    public void Construct(IDataPersistenceManager persistenceManager)
    {
        _persistenceManager = persistenceManager;
        _persistenceManager.Register(this);
    }

    private void Awake()
    {
        _itemStands = GetComponentsInChildren<ItemStand>()
            .ToList();
    }

    public bool TryPlaceHarvest(BaseHoldItem harvest)
    {
        if (harvest.TryGetComponent(out Ingredient ingredient))
        {
            var itemStand = _itemStands
                .FirstOrDefault(s => s.HeldIngredientData == ingredient.Data);

            if (itemStand == null)
            {
                Debug.LogError($"There is no itemStand for such an {ingredient}");
                return false;
            }

            return itemStand.TryReceive(harvest);
        }
        return false;
    }

    public void LoadData(GameData gameData)
    {
        foreach (var itemStandSaveData in gameData.ItemStandsSaveData)
        {
            var itemStand = _itemStands
                .FirstOrDefault(s => s.HolderId == itemStandSaveData.HolderId);
            itemStand.LoadSaveData(itemStandSaveData.CurrentPlaceCount);
        }
    }

    public void SaveData(GameData gameData)
    {
        List<ItemStandSaveData> itemStandsSaveData = new();
        foreach (var itemStand in _itemStands)
        {
            ItemStandSaveData itemStandSaveData = new();
            itemStandSaveData.HolderId = itemStand.HolderId;
            itemStandSaveData.CurrentPlaceCount = itemStand.CurrentPlaceCount;
            itemStandsSaveData.Add(itemStandSaveData);
        }
        gameData.ItemStandsSaveData = itemStandsSaveData;
    }

    private void OnDisable()
    {
        _persistenceManager.Unregister(this);
    }
}
