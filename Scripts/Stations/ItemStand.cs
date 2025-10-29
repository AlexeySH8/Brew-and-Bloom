using System.Linq;
using UnityEngine;
using Zenject;

public class ItemStand : BaseItemHolder, IDataPersistence
{
    [SerializeField] private Transform _standPoint;

    private IDataPersistenceManager _persistenceManager;
    private SpriteRenderer _itemStandVisual;

    public override Transform ParentPoint => _standPoint;
    public override int SortingOrderOffset => _itemStandVisual.sortingOrder + 1;

    [Inject]
    public void Construct(IDataPersistenceManager persistenceManager)
    {
        _persistenceManager = persistenceManager;
        _persistenceManager.Register(this);
    }

    protected override void Awake()
    {
        _itemStandVisual = GetComponent<SpriteRenderer>();
        base.Awake();
    }

    public void LoadData(GameData gameData)
    {
        ItemHolderData holderData = gameData.ItemHolders
            .FirstOrDefault(h => h.HolderId == name);

        if (holderData == null) return;

        GameObject prefab = Resources.Load<GameObject>(holderData.PrefabPath);
        var item = Instantiate(prefab);
        BaseHoldItem holdItem = item.GetComponent<BaseHoldItem>();
        TryReceive(holdItem);
    }

    public void SaveData(GameData gameData)
    {
        if (_heldItem == null) return;

        ItemHolderData holderData = gameData.ItemHolders
            .FirstOrDefault(h => h.HolderId == name);

        if (holderData == null)
        {
            holderData = new ItemHolderData() { HolderId = name };
            gameData.ItemHolders.Add(holderData);
        }
        holderData.PrefabPath = _heldItem.PrefabPath;
    }

    private void OnDisable()
    {
        _persistenceManager.Unregister(this);
    }
}
