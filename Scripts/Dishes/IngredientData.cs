using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IngredientData", menuName = "Farming/Ingredient Data")]
public class IngredientData : ScriptableObject
{
    [field: SerializeField] public IngredientType IngredientType { get; private set; }
    [field: SerializeField] public Color Color { get; private set; }
    [field: SerializeField] public GameObject IngredientPrefab { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
}
