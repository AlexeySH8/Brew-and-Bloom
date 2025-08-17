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
    protected Coroutine Coocking;

    private void Awake()
    {
        _currentIngredients = new int[_maxHeldIngredient];
    }

    public void Receive(GameObject heldItem)
    {
        if (heldItem.TryGetComponent(out Ingredient ingredient) &&
            !_unavailableIngredients.Contains(ingredient.Data))
        {
            if (!ingredient.Data)
                Debug.LogError($"{gameObject.name} has no IngredientData");

            heldItem.GetComponent<BaseHoldItem>().Discard();
            Coock(ingredient.Data);
        }
    }

    private void Coock(IngredientData ingredientData)
    {
        if (_index < _currentIngredients.Length)
        {
            _index++;
            _currentIngredients[_index] = (int)ingredientData.Ingredient;
            GameObject dish = PrepareDish();
            if (dish)
            {
                _index = 0;
                _currentIngredients = new int[_currentIngredients.Length];
                Instantiate(dish, transform.position, transform.rotation);
            }
        }
    }

    private GameObject PrepareDish()
    {
        int ingredients = 0;
        for (int i = 0; i < _currentIngredients.Length; i++)
            ingredients |= _currentIngredients[i];
        Recipes.Dishes.TryGetValue(ingredients, out var dish);
        return dish;
    }
}
