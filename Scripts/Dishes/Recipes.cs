using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "Recipes", menuName = "Farming/Recipes")]
public class Recipes : ScriptableObject
{
    [SerializeField] private List<DishData> _dishList;

    private Dictionary<int, DishData> _recipes =
        new Dictionary<int, DishData>();

    public void Initialize()
    {
        _recipes.Clear();
        foreach (var dish in _dishList)
        {
            _recipes.Add(dish.IngredientsMask, dish);
        }
    }

    public bool TryGetDish(int ingredientsMask, out GameObject dish)
    {
        dish = null;
        if (_recipes.TryGetValue(ingredientsMask, out DishData recipe))
        {
            dish = recipe.DishPrefab;
            return true;
        }
        return false;
    }

    public bool TryGetIngredients(int ingredientsMask, out IngredientData[] ingredients)
    {
        ingredients = null;
        if (_recipes.TryGetValue(ingredientsMask, out DishData recipe))
        {
            ingredients = recipe.Ingredients;
            return true;
        }
        return false;
    }

    public DishData GetRandomDish()
    {
        var dishes = _recipes.Values.ToArray();
        return dishes[Random.Range(0, dishes.Length)];
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        var assets = UnityEditor.AssetDatabase.FindAssets("t:Recipes");
        if (assets.Length > 1)
        {
            Debug.LogError("There must be only one Recipes in a project!");
        }
    }
#endif
}
