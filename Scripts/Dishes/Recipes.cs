using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public struct RecipeEntre
{
    public Ingredient[] Ingredients;
    public GameObject Dish;
}

[CreateAssetMenu(fileName = "Recipes", menuName = "Farming/Recipes")]
public class Recipes : ScriptableObject
{
    public static Dictionary<int, GameObject> Dishes;

    [SerializeField] private List<RecipeEntre> _dishesList; // list to fill in the inspector

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

    private void OnValidate()
    {
        Dishes = _dishesList.ToDictionary(
            dish => dish.Ingredients.Aggregate(0, (mask, i) => mask | (int)i.Data.IngredientType),
            dish => dish.Dish
            );
    }
}
