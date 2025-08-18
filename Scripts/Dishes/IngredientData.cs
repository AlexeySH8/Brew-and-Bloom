using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientData", menuName = "Farming/Ingredient Data")]
public class IngredientData : ScriptableObject
{
    [field: SerializeField] public Recipes.IngredientType IngredientType { get; private set; }
    [field: SerializeField] public Color Color { get; private set; }
}
