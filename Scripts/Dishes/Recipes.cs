using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Recipes", menuName = "Farming/Recipes")]
public class Recipes : ScriptableObject
{
    [SerializeField] private List<Dish> _dishList;

    private static Dictionary<int, (GameObject Dish, Ingredient[])> _recipesList =
        new Dictionary<int, (GameObject Dish, Ingredient[])>();

    public static bool TryGetDish(int ingredientsMask, out GameObject dish)
    {
        dish = null;
        if (_recipesList.TryGetValue(ingredientsMask, out (GameObject Dish, Ingredient[]) recipe))
        {
            dish = recipe.Dish;
            return true;
        }
        return false;
    }

    public static bool TryGetIngredients(int ingredientsMask, out Ingredient[] ingredients)
    {
        ingredients = null;
        if (_recipesList.TryGetValue(ingredientsMask, out (GameObject, Ingredient[] Ingredients) recipe))
        {
            ingredients = recipe.Ingredients;
            return true;
        }
        return false;
    }

    public static GameObject GetRandomDish() => _recipesList[Random.Range(0, _recipesList.Count)].Dish;

    private void OnEnable()
    {
        _recipesList.Clear();
        foreach (var dish in _dishList)
        {
            _recipesList.Add(dish.IngredientsMask, (dish.DishObject, dish.Ingredients));
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
