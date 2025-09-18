using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class CookingStation : MonoBehaviour, IReceiveHeldItem
{
    [SerializeField] private int _maxHeldIngredient;
    [SerializeField] private IngredientData[] _unavailableIngredients;

    private int _currentIngredientsMask;
    private int _index = 0;
    private CoockingStationVisual _stationVisual;
    private Coroutine _cooking;

    private void Awake()
    {
        _currentIngredientsMask = 0;
        _stationVisual = GetComponent<CoockingStationVisual>();
    }

    public void Receive(GameObject heldItem)
    {
        if (heldItem.TryGetComponent(out Ingredient ingredient) &&
            !_unavailableIngredients.Contains(ingredient.Data) &&
            _cooking == null &&
            (_currentIngredientsMask & (int)ingredient.Data.IngredientType) == 0) // the ingredient is not there yet
        {
            heldItem.GetComponent<BaseHoldItem>().Discard();
            Cook(ingredient);
        }
    }

    private void Cook(Ingredient ingredient)
    {
        if (_index >= _maxHeldIngredient)
            Clear();

        AddIngredient(ingredient);
        _stationVisual.ChangeColorTo(ingredient.Data.Color);

        if (Recipes.TryGetDish(_currentIngredientsMask, out GameObject dish))
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
