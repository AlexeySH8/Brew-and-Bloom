using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseCoockingStation : MonoBehaviour, IReceivesHeldItem
{
    [SerializeField] private SpriteRenderer _stationVisual;
    [SerializeField] private float _mixStrength = 0.5f;

    protected Coroutine Coocking;

    public void Receive(GameObject heldItem)
    {
        if (heldItem.TryGetComponent(out Ingredient ingredient))
        {
            heldItem.GetComponent<IHoldItem>().Discard();
            Coock(ingredient.Data);
        }
    }

    protected virtual void Coock(IngredientData ingredientData)
    {

    }
}
