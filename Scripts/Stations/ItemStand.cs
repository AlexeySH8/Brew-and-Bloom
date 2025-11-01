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

    public void LoadData(GameData gameData) => LoadHeldItem(gameData);

    public void SaveData(GameData gameData) => SaveHeldItem(gameData);

    private void OnDisable()
    {
        _persistenceManager.Unregister(this);
    }
}
