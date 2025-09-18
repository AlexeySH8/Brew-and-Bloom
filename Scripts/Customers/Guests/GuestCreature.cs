using UnityEngine;

public class GuestCreature : MonoBehaviour, IReceiveHeldItem
{
    private Guest _guest;

    public void Init(Guest guest)
    {
        _guest = guest;
    }

    public void Receive(GameObject heldItem)
    {
        if (_guest == null)
        {
            Debug.LogError("Guest creature is not initialized");
            return;
        }

        if (heldItem.TryGetComponent(out Dish dish))
        {
            dish.GetComponent<BaseHoldItem>().Discard();

            if (dish.Data.IngredientsMask ==
                _guest.CurrentOrder.Dish.IngredientsMask)
                _guest.CurrentOrder.MarkAsCompleted();

            _guest.StartDialogue();
        }
    }
}
