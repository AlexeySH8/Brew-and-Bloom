using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

[System.Flags]
public enum IngredientType
{
    Plate = 1 << 0,
    Skillet = 1 << 1,
    Flask = 1 << 2,
    Carrot = 1 << 3,
    Potato = 1 << 4,
    Mushroom = 1 << 5,
    Chamomile = 1 << 6,
    Onion = 1 << 7,
    Turnip = 1 << 8,
    Radish = 1 << 9,
    Spinach = 1 << 10,
    Sunflower = 1 << 11,
    Rose = 1 << 12,
    Coral = 1 << 13,
    Powder = 1 << 14
}

[CreateAssetMenu(fileName = "Recipes", menuName = "Farming/Recipes")]
public class Recipes : ScriptableObject
{
    [SerializeField] private List<DishData> _dishList;

    public Dictionary<int, DishData> RecipesDic {  get; private set; }
        = new Dictionary<int, DishData>();

    public void Initialize()
    {
        RecipesDic.Clear();
        foreach (var dish in _dishList)
        {
            if (RecipesDic.ContainsKey(dish.IngredientsMask))
            {
                Debug.LogError("A dish with these ingredients already exists. " +
                    "The ingredient mask must be unique.");
            }
            RecipesDic.Add(dish.IngredientsMask, dish);
        }
    }

    public bool TryGetDish(int ingredientsMask, out GameObject dish)
    {
        dish = null;
        if (RecipesDic.TryGetValue(ingredientsMask, out DishData recipe))
        {
            dish = recipe.DishPrefab;
            return true;
        }
        return false;
    }

    public bool TryGetIngredients(int ingredientsMask, out IngredientData[] ingredients)
    {
        ingredients = null;
        if (RecipesDic.TryGetValue(ingredientsMask, out DishData recipe))
        {
            ingredients = recipe.Ingredients;
            return true;
        }
        return false;
    }

    public DishData GetRandomDish()
    {
        var dishes = RecipesDic.Values.ToArray();
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
