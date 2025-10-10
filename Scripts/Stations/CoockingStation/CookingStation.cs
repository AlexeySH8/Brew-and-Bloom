using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using Zenject;

public class CookingStation : MonoBehaviour, IReceiveHeldItem
{
    [SerializeField] private int _maxHeldIngredient;
    [SerializeField] private IngredientData[] _unavailableIngredients;

    private int _currentIngredientsMask;
    private int _index = 0;
    private Recipes _recipes;
    private CoockingStationVisual _stationVisual;
    private Coroutine _cooking;

    [Inject]
    public void Construct(Recipes recipes)
    {
        _recipes = recipes;
    }

    private void Awake()
    {
        _currentIngredientsMask = 0;
        _stationVisual = GetComponent<CoockingStationVisual>();
    }

    public bool TryReceive(GameObject heldItem)
    {
        if (heldItem.TryGetComponent(out Ingredient ingredient) &&
            !_unavailableIngredients.Contains(ingredient.Data) &&
            _cooking == null &&
            (_currentIngredientsMask & (int)ingredient.Data.IngredientType) == 0) // the ingredient is not there yet
        {
            heldItem.GetComponent<BaseHoldItem>().Discard();
            Cook(ingredient);
            return true;
        }
        return false;
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
        }
    }

    private IEnumerator CookingDish(GameObject dish)
    {
        yield return new WaitForSeconds(0);
        Instantiate(dish, transform.position, transform.rotation);
        Clear();
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
