using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CoockingStation : MonoBehaviour, IReceivesHeldItem
{
    [SerializeField] private int _maxHeldIngredient;
    [SerializeField] private IngredientData[] _unavailableIngredients;

    private int[] _currentIngredients;
    private int _index = 0;
    private CoockingStationVisual _stationVisual;
    private Coroutine _coocking;

    private void Awake()
    {
        _currentIngredients = new int[_maxHeldIngredient];
        _stationVisual = GetComponent<CoockingStationVisual>();
    }

    public void Receive(GameObject heldItem)
    {
        if (heldItem.TryGetComponent(out Ingredient ingredient) &&
            !_unavailableIngredients.Contains(ingredient.Data) &&
            _coocking == null &&
            !_currentIngredients.Contains((int)ingredient.Data.IngredientType))
        {

            if (!ingredient.Data)
                Debug.LogError($"{gameObject.name} has no IngredientData");

            heldItem.GetComponent<BaseHoldItem>().Discard();
            Cook(ingredient);
        }
    }

    private void Cook(Ingredient ingredient)
    {
        if (_index >= _currentIngredients.Length)
            Clear();

        AddIngredient(ingredient);
        _stationVisual.ChangeColorTo(ingredient.Data.Color);

        if (TryCoockDish(out GameObject dish))
        {
            _coocking = StartCoroutine(CoockDish(dish));
        }
    }

    private IEnumerator CoockDish(GameObject dish)
    {
        yield return new WaitForSeconds(3);
        Instantiate(dish, transform.position, transform.rotation);
        Clear();
    }

    private void AddIngredient(Ingredient ingredient)
    {
        _currentIngredients[_index] = (int)ingredient.Data.IngredientType;
        _stationVisual.AddIngredient(ingredient.GetComponent<SpriteRenderer>().sprite);
        _index++;
    }

    private bool TryCoockDish(out GameObject dish)
    {
        int ingredients = 0;
        for (int i = 0; i < _currentIngredients.Length; i++)
            ingredients |= _currentIngredients[i];
        return Recipes.Dishes.TryGetValue(ingredients, out dish);
    }

    private void Clear()
    {
        _currentIngredients = new int[_maxHeldIngredient];
        _index = 0;
        _coocking = null;
        _stationVisual.ClearIngredients();
    }
}
