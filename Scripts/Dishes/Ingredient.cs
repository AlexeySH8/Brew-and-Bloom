using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ingredient : BaseHoldItem
{
    [field: SerializeField] public IngredientData Data { get; private set; }

    public Sprite Icon { get => GetComponent<SpriteRenderer>().sprite; }
}
