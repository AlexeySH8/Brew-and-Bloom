using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipes", menuName = "Farming/Recipes")]
public class Recipes : ScriptableObject
{
    [SerializeField] private List<DishData> _dishList;

    private static Dictionary<int, DishData> _recipes =
        new Dictionary<int, DishData>();

    public static bool TryGetDish(int ingredientsMask, out GameObject dish)
    {
        dish = null;
        if (_recipes.TryGetValue(ingredientsMask, out DishData recipe))
        {
            dish = recipe.DishPrefab;
            return true;
        }
        return false;
    }

    public static bool TryGetIngredients(int ingredientsMask, out IngredientData[] ingredients)
    {
        ingredients = null;
        if (_recipes.TryGetValue(ingredientsMask, out DishData recipe))
        {
            ingredients = recipe.Ingredients;
            return true;
        }
        return false;
    }

    public static DishData GetRandomDish()
    {
        var dishes = _recipes.Values.ToArray();
        return dishes[Random.Range(0, dishes.Length)];
    }

    private void OnEnable()
    {
        _recipes.Clear();
        foreach (var dish in _dishList)
        {
            _recipes.Add(dish.IngredientsMask, dish);
        }
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
