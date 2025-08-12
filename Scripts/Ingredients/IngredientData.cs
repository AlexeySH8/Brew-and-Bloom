using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientData", menuName = "Farming/Ingredient Data")]
public class IngredientData : ScriptableObject
{
    [field: SerializeField] public Recipes.Ingredient Ingredient { get; private set; }
    [field: SerializeField] public Ingredient Color { get; private set; }
}
