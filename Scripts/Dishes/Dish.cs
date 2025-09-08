using UnityEngine;

public class Dish : BaseHoldItem
{
    [field: SerializeField] public Ingredient[] Ingredients { get; private set; }
    [field: SerializeField] public int IngredientsMask { get; private set; }

    public GameObject DishObject { get => gameObject; }

    private void OnValidate()
    {
        IngredientsMask = 0;
        if (Ingredients == null) return;

        foreach (var ingredient in Ingredients)
            IngredientsMask |= (int)ingredient.Data.IngredientType;
    }
}
