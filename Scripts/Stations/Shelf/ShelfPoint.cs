using UnityEngine;
using Zenject;

public class ShelfPoint : BaseItemHolder, IDataPersistence
{
    [SerializeField] private BaseUsableItem _toolPref;

    public override Transform ParentPoint => transform;
    public override int SortingOrderOffset =>
        transform.parent.GetComponent<SpriteRenderer>().sortingOrder + 1;

    private bool _isToolSpawned = false;
    private IDataPersistenceManager _persistenceManager;

    [Inject]
    public void Construct(IDataPersistenceManager persistenceManager)
    {
        _persistenceManager = persistenceManager;
        _persistenceManager.Register(this);
    }

    private void Start()
    {
        if (!_isToolSpawned)
        {
            GameObject tool = Instantiate(_toolPref.gameObject);
            TryReceive(tool.GetComponent<BaseHoldItem>());
            _isToolSpawned = true;
        }
    }

    protected override bool IsCorrectItemToReceive(BaseHoldItem heldItem)
    {
        return heldItem.TryGetComponent(out BaseUsableItem usableItem) &&
            usableItem.InteractionMask == _toolPref.InteractionMask;
    }

    public void LoadData(GameData gameData)
    {
        _isToolSpawned = gameData.IsToolSpawned;
        LoadHeldItem(gameData);
    }

    public void SaveData(GameData gameData)
    {
        gameData.IsToolSpawned = _isToolSpawned;
        SaveHeldItem(gameData);
    }

    private void OnDisable()
    {
        _persistenceManager.Unregister(this);
    }
}
