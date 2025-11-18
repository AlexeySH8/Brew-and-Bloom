using UnityEngine;
using Zenject;

public class ItemStand : BaseItemHolder
{
    public int MaxPlaceCount { get; private set; } = 5;
    public int CurrentPlaceCount { get; private set; }
    [SerializeField] private IngredientData _heldIngredientData;
    [SerializeField] private Transform _standPoint;
    // private IDataPersistenceManager _persistenceManager;
    private ItemStandVisual _itemStandVisual;

    public override Transform ParentPoint => _standPoint;
    public override int SortingOrderOffset => _itemStandVisual.SortingOrderOffset;
    public IngredientData HeldIngredientData => _heldIngredientData;

    //[Inject]
    //public void Construct(IDataPersistenceManager persistenceManager)
    //{
    //    _persistenceManager = persistenceManager;
    //    _persistenceManager.Register(this);
    //}

    protected override void Awake()
    {
        _itemStandVisual = GetComponent<ItemStandVisual>();
        base.Awake();
    }

    public override bool TryReceive(BaseHoldItem heldItem)
    {
        if (CurrentPlaceCount < MaxPlaceCount &&
            IsCorrectItemToReceive(heldItem))
        {
            CurrentPlaceCount++;
            heldItem.Discard();
            _itemStandVisual.UpdateVisual(CurrentPlaceCount, _heldIngredientData.Icon);
            return true;
        }
        return false;
    }

    public override BaseHoldItem GiveItem()
    {
        if (CurrentPlaceCount > 0)
        {
            CurrentPlaceCount--;
            _itemStandVisual.UpdateVisual(CurrentPlaceCount, _heldIngredientData.Icon);
            GameObject ingredientObj = Instantiate(_heldIngredientData.IngredientPrefab);
            return ingredientObj.GetComponent<BaseHoldItem>();
        }
        return null;
    }

    public void LoadSaveData(int currentPlaceCount)
    {
        CurrentPlaceCount = currentPlaceCount;
        _itemStandVisual.UpdateVisual(CurrentPlaceCount, _heldIngredientData.Icon);
    }

    protected override bool IsCorrectItemToReceive(BaseHoldItem heldItem)
    {
        if (heldItem.TryGetComponent(out Ingredient ingredient))
            return ingredient.Data == _heldIngredientData;
        return false;
    }

    //private void OnDisable()
    //{
    //    _persistenceManager.Unregister(this);
    //}
}
