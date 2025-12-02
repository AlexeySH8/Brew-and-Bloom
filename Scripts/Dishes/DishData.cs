using UnityEngine;

public enum CoockingStationType
{
    Cauldron,
    Fireplace
}

[CreateAssetMenu(fileName = "NewDishData", menuName = "Farming/Dish Data")]
public class DishData : ScriptableObject
{
    [field: SerializeField] public CoockingStationType CoockingStationType { get; private set; }
    [field: SerializeField] public IngredientData[] Ingredients { get; private set; }
    [field: SerializeField] public int IngredientsMask { get; private set; }
    [field: SerializeField] public GameObject DishPrefab { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }

    private void OnValidate()
    {
        IngredientsMask = 0;
        if (Ingredients == null) return;

        foreach (var ingredient in Ingredients)
            IngredientsMask |= (int)ingredient.IngredientType;
    }
}


