using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

[CreateAssetMenu(fileName = "IngredientData", menuName = "Farming/Ingredient Data")]
public class IngredientData : ScriptableObject
{
    [field: SerializeField] public IngredientType IngredientType { get; private set; }
    [field: SerializeField] public Color Color { get; private set; }
    [field: SerializeField] public GameObject IngredientPrefab { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
}
