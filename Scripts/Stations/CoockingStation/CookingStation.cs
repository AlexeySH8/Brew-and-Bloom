using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;

public class CookingStation : BaseItemHolder
{
    public bool IsCookingStop;
    [SerializeField] private int _maxHeldIngredient;
    [SerializeField] private Transform _dishHolder;
    [SerializeField] private IngredientData[] _unavailableIngredients;
    [SerializeField] private float _minCookingTime;
    [SerializeField] private float _maxCookingTime;

    private int _currentIngredientsMask;
    private int _index = 0;
    private Recipes _recipes;
    private CoockingStationVisual _stationVisual;
    private Coroutine _cooking;

    public override Transform ParentPoint => _dishHolder;

    public override int SortingOrderOffset
        => _stationVisual.SpriteRenderer.sortingOrder + 1;

    [Inject]
    public void Construct(Recipes recipes)
    {
        _recipes = recipes;
    }

    private void Awake()
    {
        IsCookingStop = false;
        _currentIngredientsMask = 0;
        _stationVisual = GetComponent<CoockingStationVisual>();
    }

    public override bool TryReceive(BaseHoldItem heldItem)
    {
        if (heldItem.TryGetComponent(out Ingredient ingredient) &&
            !_unavailableIngredients.Contains(ingredient.Data) &&
            _cooking == null &&
            (_currentIngredientsMask & (int)ingredient.Data.IngredientType) == 0) // the ingredient is not there yet
        {
            heldItem.Discard();
            Cook(ingredient);
            return true;
        }
        return false;
    }

    public override BaseHoldItem GiveItem()
    {
        var heldItem = base.GiveItem();
        if (heldItem != null)
            Clear();
        return heldItem;
    }

    private void Cook(Ingredient ingredient)
    {
        if (_index >= _maxHeldIngredient)
            Clear();

        AddIngredient(ingredient);
        _stationVisual.ChangeColorTo(ingredient.Data.Color);

        if (_recipes.TryGetDish(_currentIngredientsMask, out GameObject dish))
        {
            _cooking = StartCoroutine(CookingDish(dish));
            _stationVisual.ClearIngredients();
        }
    }

    private IEnumerator CookingDish(GameObject dish)
    {
        float duration = UnityEngine.Random.Range(_minCookingTime, _maxCookingTime);
        float elapsed = 0;
        while (elapsed < duration)
        {
            if (!IsCookingStop)
                elapsed += Time.deltaTime;
            yield return null;
        }
        SpawnDish(dish);
    }

    private void SpawnDish(GameObject dish)
    {
        BaseHoldItem holdItem = Instantiate(
            dish, transform.position, transform.rotation)
            .GetComponent<BaseHoldItem>();
        base.TryReceive(holdItem);
    }

    private void AddIngredient(Ingredient ingredient)
    {
        _currentIngredientsMask |= (int)ingredient.Data.IngredientType;
        _stationVisual.AddIngredient(ingredient.Data.Icon);
        _index++;
    }

    private void Clear()
    {
        _currentIngredientsMask = 0;
        _index = 0;
        _cooking = null;
        _stationVisual.ClearIngredients();
    }
}
